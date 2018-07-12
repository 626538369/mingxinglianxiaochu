using System;
using System.Net;
using System.Threading;

using System.IO;

/// <summary>
/// Upload step delegate
/// </summary>
public delegate void UploadStepDelegate(long _stepBytes);

public class FtpState
{
    private FtpWebRequest request;
    private string fileName;
    private Exception operationException = null;
	private UploadStepDelegate uploadProgressDelegate = null;
	
    private string status = null;
	    
    public FtpState()
    {
    }    
    
    public FtpWebRequest Request
    {
        get {return request;}
        set {request = value;}
    }
    
    public string FileName
    {
        get {return fileName;}
        set {fileName = value;}
    }
    public Exception OperationException
    {
        get {return operationException;}
        set {operationException = value;}
    }
    public string StatusDescription
    {
        get {return status;}
        set {status = value;}
    }
	
	public UploadStepDelegate StepProgressDelegate{
		get{return uploadProgressDelegate;}
		set{uploadProgressDelegate = value;} 
	}
}

/// <summary>
/// Asynchronous ftp up loader.
/// copy from http://msdn.microsoft.com/zh-cn/library/system.net.ftpwebrequest%28v=VS.80%29.aspx
/// and been modified by tzz
/// date: 2012-9-28
/// </summary>
public class AsynchronousFtpUpLoader{  
	
	private FtpWebRequest request;
	private FtpState state;
		
	public FtpState State{
		get{return state;}
	}
	
	/// <summary>
	/// Stop this ftp uploader
	/// </summary>
	public void Stop(){
		request.Abort();
	}
		
    // Command line arguments are two strings:
    // 1. The url that is the name of the file being uploaded to the server.
    // 2. The name of the file on the local machine.
    //
    public FtpState Upload(string _file,string _uri,string _account,string _pass,UploadStepDelegate _stepDelegate){
				
        state = new FtpState();
		
        request = (FtpWebRequest)WebRequest.Create(new Uri (_uri + "/" + Path.GetFileName(_file)));
        request.Method = WebRequestMethods.Ftp.UploadFile;
        
        // This example uses anonymous logon.
        // The request is anonymous by default; the credential does not have to be specified. 
        // The example specifies the credential only to
        // control how actions are logged on the server.
        
        request.Credentials = new NetworkCredential(_account,_pass);
        
        // Store the request in the object that we pass into the
        // asynchronous operations.
        state.Request 				=	request;
        state.FileName				= _file;
		state.StepProgressDelegate	= _stepDelegate;
         
        // Asynchronously get the stream for the file contents.
        request.BeginGetRequestStream(
            new AsyncCallback (EndGetStreamCallback), 
            state
        );
		
		return state;
    }
	
    private void EndGetStreamCallback(IAsyncResult ar)
    {
        FtpState state = (FtpState) ar.AsyncState;
        
        Stream requestStream = null;
        // End the asynchronous call to get the request stream.
        try
        {
            requestStream = state.Request.EndGetRequestStream(ar);
            // Copy the file contents to the request stream.
            const int bufferLength = 2048;
            byte[] buffer = new byte[bufferLength];
            int count = 0;
            int readBytes = 0;
            FileStream stream = File.OpenRead(state.FileName);
            do
            {
                readBytes = stream.Read(buffer, 0, bufferLength);
                requestStream.Write(buffer, 0, readBytes);
                count += readBytes;
				
				if(state.StepProgressDelegate != null){
					state.StepProgressDelegate(count);
				}
            }
            while (readBytes != 0);
            
			Debug.Log ("Writing {0} bytes to the stream.", count);
			
            // IMPORTANT: Close the request stream before sending the request.
            requestStream.Close();
			
            // Asynchronously get the response to the upload request.
            state.Request.BeginGetResponse(
                new AsyncCallback (EndGetResponseCallback), 
                state
            );
        } 
        // Return exceptions to the main application thread.
        catch (Exception e)
        {
            Debug.LogError("AsynchronousFtpUpLoader Could not get the request stream. " + e.Message);	
            state.OperationException = e;            
        }       
    }
    
    // The EndGetResponseCallback method  
    // completes a call to BeginGetResponse.
    private void EndGetResponseCallback(IAsyncResult ar)
    {
        FtpState state = (FtpState) ar.AsyncState;
        FtpWebResponse response = null;
        try 
        {
            response = (FtpWebResponse) state.Request.EndGetResponse(ar);
            response.Close();
			
            state.StatusDescription = response.StatusDescription;
        }
        // Return exceptions to the main application thread.
        catch (Exception e)
        {
            Debug.LogError("AsynchronousFtpUpLoader Error getting response." + e.Message);
            state.OperationException = e;
        }
	}
}


