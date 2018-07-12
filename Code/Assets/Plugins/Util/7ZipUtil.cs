using UnityEngine;
using System.Collections;
using SevenZip.Compression.LZMA;
using System.IO;
using System;

[ExecuteInEditMode]
[System.Serializable]
public class ZipUtil 
{
	public static void CompressFileLZMA(string inFile, string outFile)
	{
		SevenZip.Compression.LZMA.Encoder coder = new SevenZip.Compression.LZMA.Encoder();
		FileStream input = new FileStream(inFile, FileMode.Open);
		FileStream output = new FileStream(outFile, FileMode.Create);
		
		// Write the encoder properties
		coder.WriteCoderProperties(output);
		
		// Write the decompressed file size.
		output.Write(BitConverter.GetBytes(input.Length), 0, 8);
		
		// Encode the file.
		coder.Code(input, output, input.Length, -1, null);
		output.Flush();
		output.Close();
		input.Close();
	}
	
	public static void DecompressFileLZMA(string inFile, string outFile)
	{
		Debug.Log("DecompressFileLZMA in File is :" + inFile);
		Debug.Log("DecompressFileLZMA in outFile is :" + outFile);
		SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();
		FileStream input = new FileStream(inFile, FileMode.Open,FileAccess.Read);
		string path = outFile.Substring(0,outFile.LastIndexOf("/"));
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		FileStream output = new FileStream(outFile, FileMode.Create);
		
		// Read the decoder properties
		byte[] properties = new byte[5];
		input.Read(properties, 0, 5);
		
		// Read in the decompress file size.
		byte [] fileLengthBytes = new byte[8];
		input.Read(fileLengthBytes, 0, 8);
		long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);
 
		// Decompress the file.
		coder.SetDecoderProperties(properties);
		coder.Code(input, output, input.Length, fileLength, null);
		output.Flush();
		output.Close();
		input.Close();
	}
	
	public static void DecompressByteLZMA(byte[] inFileBytes, string outFile)
	{
		SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();


		MemoryStream memoryStream = new MemoryStream(inFileBytes);

		string path = outFile.Substring(0,outFile.LastIndexOf("/"));
		
		Debug.Log("OutFile is " + path);
		
		if (!Directory.Exists(path))
		{
			Debug.Log("CreateDirectory : " + path);
			Directory.CreateDirectory(path);
		}


		FileStream output = new FileStream(outFile, FileMode.Create);


		// Read the decoder properties
		byte[] properties = new byte[5];
		memoryStream.Read(properties, 0, 5);
		
		// Read in the decompress file size.
		byte [] fileLengthBytes = new byte[8];
		memoryStream.Read(fileLengthBytes, 0, 8);
		long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);
 
		// Decompress the file.
		coder.SetDecoderProperties(properties);
		coder.Code(memoryStream, output, memoryStream.Length, fileLength, null);
		output.Flush();
		output.Close();



	}
	
}
