/*
 * MIT License
 * 
 * Copyright (c) 2025 Runic Compiler Toolkit Contributors
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System.IO.Compression;
using System.Text;

Console.WriteLine("[INFO] Creating nuget package ...");

static string CreateNuspec(string packageId, string version, string description, string tags)
{
    var stringBuilder = new StringBuilder();
    stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
    stringBuilder.AppendLine("<package xmlns=\"http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd\">");
    stringBuilder.AppendLine("  <metadata>");
    stringBuilder.AppendLine("    <id>" + packageId + "</id>");
    stringBuilder.AppendLine("    <version>" + version + "</version>");
    stringBuilder.AppendLine("    <title>" + packageId + "</title>");
    stringBuilder.AppendLine("    <authors>runicct</authors>");
    stringBuilder.AppendLine("    <owners>runicct</owners>");
    stringBuilder.AppendLine("    <description>" + description + "</description>");
    stringBuilder.AppendLine("    <projectUrl>https://github.com/runicct/runic-cil-disassembler</projectUrl>");
    stringBuilder.AppendLine("    <language>en-US</language>");
    stringBuilder.AppendLine("    <tags>runic runicct compiler construction toolkit " + tags + "</tags>");
    stringBuilder.AppendLine("    <license type=\"expression\">MIT</license>");
    stringBuilder.AppendLine("  </metadata>");
    stringBuilder.AppendLine("</package>");
    return stringBuilder.ToString();
}
string version = "1.0.0";
string currentExeDir = System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(System.Environment.ProcessPath));
string nupkgPath = System.IO.Path.GetFullPath(currentExeDir + "/runic.cil.disassembler." + version + ".nupkg");
string directoryName = System.IO.Path.GetFileName(System.IO.Path.GetFullPath(currentExeDir + " /.."));
string assemblyName = "Runic.CIL.Disassembler.dll";
string rootDir = System.IO.Path.GetFullPath(currentExeDir + "/../../../..");
string net6 = System.IO.Path.GetFullPath(rootDir + "/Runic.CIL.Disassembler.Net6/bin/" + directoryName + "/net6.0/" + assemblyName);
string net8 = System.IO.Path.GetFullPath(rootDir + "/Runic.CIL.Disassembler.Net8/bin/" + directoryName + "/net8.0/" + assemblyName);
string net48 = System.IO.Path.GetFullPath(rootDir + "/Runic.CIL.Disassembler.Net48/bin/" + directoryName + "/" + assemblyName);

Console.WriteLine("[INFO] Writing package at: " + nupkgPath);

if (System.IO.File.Exists(nupkgPath)) { System.IO.File.Delete(nupkgPath); }
using (var fileStream = new FileStream(nupkgPath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
{
    using (var zip = new ZipArchive(fileStream, ZipArchiveMode.Create, false, Encoding.UTF8))
    {
        var entry = zip.CreateEntry("Runic.CIL.Disassembler.nuspec", CompressionLevel.Optimal);
        using (var stream = entry.Open())
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(CreateNuspec("Runic.CIL.Disassembler", "1.0.0", "This package is part of Runic Compiler Toolkit and provides a way to disassemble CIL programs", "cil msil il bytecode disassembler"));
            stream.Write(bytes, 0, bytes.Length);
        }
        entry = zip.CreateEntry("lib/net4.8/Runic.CIL.Disassembler.dll", CompressionLevel.Optimal);
        using (var stream = entry.Open())
        {
            byte[] bytes = File.ReadAllBytes(net48);
            stream.Write(bytes, 0, bytes.Length);
        }

        entry = zip.CreateEntry("lib/net6/Runic.CIL.Disassembler.dll", CompressionLevel.Optimal);
        using (var stream = entry.Open())
        {
            byte[] bytes = File.ReadAllBytes(net6);
            stream.Write(bytes, 0, bytes.Length);
        }

        entry = zip.CreateEntry("lib/net8/CLICC.CIL.Disassembler.dll", CompressionLevel.Optimal);
        using (var stream = entry.Open())
        {
            byte[] bytes = File.ReadAllBytes(net6);
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}