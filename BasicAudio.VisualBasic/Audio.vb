Namespace Argus.Audio

    ''' <summary>
    ''' Recording via the current audio device
    ''' </summary>
    ''' <remarks>
    ''' This has been updated to work with Vista's updated mciSendString API's (the legacy call
    ''' continued to work but would only record 8bit 11kpbs which is horrible quality, had to add
    ''' the alignment, samplespersec and bytespersec in that particular order to get it to work
    ''' as per the only single post I could find on the web that had content that fixed the issue).
    '''
    ''' If any latency issues occur, look into whether your sound card supports ASIO drivers. 
    ''' 
    ''' VB Usage:
    '''    Dim recorder As New Argus.Audio.Recording("C:\test.wav")
    '''    recorder.StartRecording  ' to stop, invoke the StopRecording sub
    ''' C# usage:
    '''    var recorder = new Argus.Audio.Recording(@"C:\test.wav");
    '''    recorder.StartRecording();
    ''' </remarks>
    Public Class Recording

        '*********************************************************************************************************************
        '
        '             Class:  Recording
        '      Organization:  http://www.blakepell.com     
        '      Initial Date:  03/31/2007
        '      Last Updated:  04/05/2019
        '     Programmer(s):  Blake Pell, blakepell@hotmail.com
        '
        '*********************************************************************************************************************

        ' Windows API Calls
        Private Declare Function mciSendString Lib "winmm.dll" Alias "mciSendStringA" (ByVal lpstrCommand As String, ByVal lpstrReturnString As String, ByVal uReturnLength As Int32, ByVal hwndCallback As Int32) As Int32
        Private Declare Function waveOutGetNumDevs Lib "winmm.dll" () As Integer

        Private _timeElapsed As New Stopwatch()

        ''' <summary>
        ''' Constructor:  The WavFileName initializes the file that should be saved when the stop method is called.
        ''' </summary>
        ''' <param name="WavFileName">This is the file that will be saved when the StopRecording method is called.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal wavFileName As String)
            Me.Filename = wavFileName
        End Sub

        ''' <summary>
        ''' Constructor:  If you use the blank constructor you will need to set the wave filename manually before you stop recording.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Start recording
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub StartRecording()

            If IsRecording = True Then
                Throw New Exception("You are already recording.")
                Exit Sub
            End If

            'Calculate the average bytes/sec and block alignment  
            Dim averageBytes As Integer = BitsPerSample * Channels * SamplesPerSecond / 8
            Dim alignment As Integer = BitsPerSample * Channels / 8

            'Even though MCI's documentation does not mention these need to be set in a certain order, I've found that if  
            'the below order is not used, the function will fail.  

            ' I tried to use a string builder here but it kept pulling the text from the enum and not the value.  :P
            Dim command As String = "set capture bitspersample " & BitsPerSample & " channels " & Channels

            command += " alignment " & alignment & " samplespersec " & SamplesPerSecond & " bytespersec " & averageBytes
            command += " format tag pcm wait"

            Dim returnValue As Integer = mciSendString("close capture", 0&, 0, 0)
            returnValue = mciSendString("open new type waveaudio alias capture", 0&, 0, 0)
            returnValue = mciSendString(command, 0&, 0, 0)
            returnValue = mciSendString("record capture", 0&, 0, 0)

            IsRecording = True

            _timeElapsed.Reset()
            _timeElapsed.Start()

        End Sub

        ''' <summary>
        ''' Stop recording.  This will also save the file to disk that was previously specified (or should have been previously specified).  
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub StopRecording()
            If _filename = "" Then
                Throw New Exception("No file specified to save to.")
            End If

            Dim returnValue As Integer = mciSendString("stop capture", 0&, 0, 0)
            returnValue = mciSendString("save capture " & _filename, 0&, 0, 0)
            returnValue = mciSendString("close capture", 0&, 0, 0)
            IsRecording = False

            _timeElapsed.Stop()
        End Sub

        ''' <summary>
        ''' Whether or not a sound card exists.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SoundCardExist() As Boolean
            Dim waveOutDevices As Integer = waveOutGetNumDevs()

            If waveOutDevices > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Private _isPaused As Boolean = False
        ''' <summary>
        ''' Gets or sets whether the currently recording is paused or not.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property Pause() As Boolean
            Get
                Return _isPaused
            End Get
            Set(ByVal value As Boolean)
                If IsRecording = False Then
                    _isPaused = False
                    Exit Property
                End If

                _isPaused = value

                If _isPaused = True Then
                    Dim returnValue As Int32 = 0
                    returnValue = mciSendString("pause capture", 0&, 0, IntPtr.Zero)
                    _timeElapsed.Stop()
                Else
                    Dim returnValue As Int32 = 0
                    returnValue = mciSendString("resume capture", 0&, 0, IntPtr.Zero)
                    _timeElapsed.Start()
                End If

            End Set
        End Property

        ''' <summary>
        ''' Channel settings.  Mono and Stereo are the only supported at this time.
        ''' </summary>
        ''' <remarks></remarks>
        Enum ChannelValue
            MONO = 1
            STEREO = 2
        End Enum
        ''' <summary>
        ''' The channel property.  The default value is stereo.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Channels() As ChannelValue = ChannelValue.STEREO

        ''' <summary>
        ''' Samples per second values.  The current supported values are 11025, 22050 and 44100 (Low, Medium and High)
        ''' </summary>
        ''' <remarks></remarks>
        Enum SamplesPerSecValue
            LOW = 11025
            MEDIUM = 22050
            HIGH = 44100
        End Enum
        ''' <summary>
        ''' The samples per second property.  The default value is high or 44100 samples per second (CD Quality).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SamplesPerSecond() As SamplesPerSecValue = SamplesPerSecValue.HIGH

        ''' <summary>
        ''' The bits per second value
        ''' </summary>
        ''' <remarks></remarks>
        Enum BitsPerSampleValue
            LOW = 8
            HIGH = 16
        End Enum

        ''' <summary>
        ''' The bits per second property.  The default value is high or 16-bit.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BitsPerSample() As BitsPerSampleValue = BitsPerSampleValue.HIGH

        Private _filename As String = ""
        ''' <summary>
        ''' The filename property.  This is the file that will be saved to whenever the StopRecording method is called.  If the
        ''' ForceExtension property is set to true (which it is be default) then it will force the filename to have the proper .wav 
        ''' extension.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Filename() As String
            Get
                Return _filename
            End Get
            Set(ByVal value As String)
                ' Check if we should force putting the .wav extension on the file, but allow the user
                ' to disable this if they want
                If ForceExtension = True Then
                    If value.Length > 4 And Right(value.ToLower, 4) <> ".wav" Then
                        value = value & ".wav"
                    End If
                End If

                _filename = value
            End Set
        End Property
        ''' <summary>
        ''' A property that determines whether the class will force the file to have the .wav extension (it will add it if you don't).  The default
        ''' value for this is true.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ForceExtension() As Boolean = True

        ''' <summary>
        ''' The time elapsed on the current recording.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property TimeElapsed() As Long
            Get
                Return _timeElapsed.ElapsedMilliseconds
            End Get
        End Property

        ''' <summary>
        ''' The current number of bytes that are stored in memory.  This property obtains it's value by making a call to the
        ''' Windows API mciSendString.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property BytesInMemory() As String
            Get
                Dim buf As String = ""
                Dim returnValue As Integer = 0

                buf = Space(255)
                returnValue = mciSendString("set capture time format bytes", 0&, 0, 0)
                returnValue = mciSendString("status capture length", buf, 255, 0)
                Return buf
            End Get
        End Property

        ''' <summary>
        ''' Whether or not the class is currently recording.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IsRecording() As Boolean = False

    End Class

    ''' <summary>
    ''' This class is a wrapper for the Windows API calls to play wave, midi or mp3 files.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    Public Class AudioFile

        '*********************************************************************************************************************
        '
        '             Class:  AudioFile
        '      Organization:  http://www.blakepell.com     
        '      Initial Date:  03/31/2007
        '      Last Updated:  02/04/2009
        '     Programmer(s):  Blake Pell, blakepell@hotmail.com
        '
        '*********************************************************************************************************************

        ' Windows API Declarations
        Private Declare Function mciSendString Lib "winmm.dll" Alias "mciSendStringA" (ByVal lpstrCommand As String, ByVal lpstrReturnString As String, ByVal uReturnLength As Int32, ByVal hwndCallback As Int32) As Int32

        ''' <summary>
        ''' Constructor:  Location is the filename of the media to play.  Wave files and Mp3 files are the supported formats.
        ''' </summary>
        ''' <param name="Location"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal location As String)
            Me.Filename = location
        End Sub

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Plays the file that is specified as the filename.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Play()

            If _filename = "" Or Filename.Length <= 4 Then Exit Sub

            Select Case Right(Filename, 3).ToLower
                Case "mp3"
                    mciSendString("open """ & _filename & """ type mpegvideo alias audiofile", Nothing, 0, IntPtr.Zero)

                    Dim playCommand As String = "play audiofile from 0"

                    If Wait = True Then playCommand += " wait"

                    mciSendString(playCommand, Nothing, 0, IntPtr.Zero)
                Case "wav"
                    mciSendString("open """ & _filename & """ type waveaudio alias audiofile", Nothing, 0, IntPtr.Zero)
                    mciSendString("play audiofile from 0", Nothing, 0, IntPtr.Zero)
                Case "mid", "idi"
                    mciSendString("stop midi", "", 0, 0)
                    mciSendString("close midi", "", 0, 0)
                    mciSendString("open sequencer!" & _filename & " alias midi", "", 0, 0)
                    mciSendString("play midi", "", 0, 0)
                Case Else
                    Throw New Exception("File type not supported.")
                    Call Close()
            End Select

            IsPaused = False

        End Sub

        ''' <summary>
        ''' Pause the current play back.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Pause()
            mciSendString("pause audiofile", Nothing, 0, IntPtr.Zero)
            IsPaused = True
        End Sub

        ''' <summary>
        ''' Resume the current play back if it is currently paused.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub [Resume]()
            mciSendString("resume audiofile", Nothing, 0, IntPtr.Zero)
            IsPaused = False
        End Sub

        ''' <summary>
        ''' Stop the current file if it's playing.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub [Stop]()
            mciSendString("stop audiofile", Nothing, 0, IntPtr.Zero)
        End Sub

        ''' <summary>
        ''' Close the file.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Close()
            mciSendString("close audiofile", Nothing, 0, IntPtr.Zero)
        End Sub

        ''' <summary>
        ''' Halt the program until the .wav file is done playing.  Be careful, this will lock the entire program up until the
        ''' file is done playing.  It behaves as if the Windows Sleep API is called while the file is playing (and maybe it is, I don't
        ''' actually know, I'm just theorizing).  :P
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Wait() As Boolean = False

        ''' <summary>
        ''' Sets the audio file's time format via the mciSendString API.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Milleseconds() As Integer
            Get
                Dim buf As String = Space(255)
                mciSendString("set audiofile time format milliseconds", Nothing, 0, IntPtr.Zero)
                mciSendString("status audiofile length", buf, 255, IntPtr.Zero)

                buf = Replace(buf, Chr(0), "") ' Get rid of the nulls, they muck things up

                If buf = "" Then
                    Return 0
                Else
                    Return CInt(buf)
                End If
            End Get
        End Property

        ''' <summary>
        ''' Gets the status of the current playback file via the mciSendString API.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Status() As String
            Get
                Dim buf As String = Space(255)
                mciSendString("status audiofile mode", buf, 255, IntPtr.Zero)
                buf = Replace(buf, Chr(0), "")  ' Get rid of the nulls, they muck things up
                Return buf
            End Get
        End Property

        ''' <summary>
        ''' Gets the file size of the current audio file.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property FileSize() As Integer
            Get
                Try
                    Return My.Computer.FileSystem.GetFileInfo(_filename).Length
                Catch ex As Exception
                    Return 0
                End Try
            End Get
        End Property

        ''' <summary>
        ''' Gets the channels of the file via the mciSendString API.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Channels() As Integer
            Get
                Dim buf As String = Space(255)
                mciSendString("status audiofile channels", buf, 255, IntPtr.Zero)

                If IsNumeric(buf) = True Then
                    Return CInt(buf)
                Else
                    Return -1
                End If
            End Get
        End Property

        ''' <summary>
        ''' Used for debugging purposes.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Debug() As String
            Get
                Dim buf As String = Space(255)
                mciSendString("status audiofile channels", buf, 255, IntPtr.Zero)

                Return Str(buf)
            End Get
        End Property
        ''' <summary>
        ''' Whether or not the current playback is paused.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IsPaused() As Boolean = False

        Private _filename As String
        ''' <summary>
        ''' The current filename of the file that is to be played back.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Filename() As String
            Get
                Return _filename
            End Get
            Set(ByVal value As String)

                If My.Computer.FileSystem.FileExists(value) = False Then
                    Throw New Exception("File does not exist.")
                    Exit Property
                End If

                _filename = value
            End Set
        End Property

    End Class

    ''' <summary>
    ''' This is a lookup class to identify the MCI errors that may occur.
    ''' </summary>
    ''' <remarks>
    ''' There's is an API that will also do this for us, We should consider using that in the future.
    ''' </remarks>
    Public Class MCIError

        '*********************************************************************************************************************
        '
        '             Class:  MCIError
        '      Organization:  http://www.blakepell.com     
        '      Initial Date:  03/31/2007
        '      Last Updated:  03/31/2007
        '     Programmer(s):  Blake Pell, blakepell@hotmail.com
        '
        '*********************************************************************************************************************

        ''' <summary>
        ''' The device error enums
        ''' </summary>
        ''' <remarks></remarks>
        Enum MCIERR
            MCIERR_BASE = 256
            MCIERR_INVALID_DEVICE_ID = (MCIERR_BASE + 1)
            MCIERR_UNRECOGNIZED_KEYWORD = (MCIERR_BASE + 3)
            MCIERR_UNRECOGNIZED_COMMAND = (MCIERR_BASE + 5)
            MCIERR_HARDWARE = (MCIERR_BASE + 6)
            MCIERR_INVALID_DEVICE_NAME = (MCIERR_BASE + 7)
            MCIERR_OUT_OF_MEMORY = (MCIERR_BASE + 8)
            MCIERR_DEVICE_OPEN = (MCIERR_BASE + 9)
            MCIERR_CANNOT_LOAD_DRIVER = (MCIERR_BASE + 10)
            MCIERR_MISSING_COMMAND_STRING = (MCIERR_BASE + 11)
            MCIERR_PARAM_OVERFLOW = (MCIERR_BASE + 12)
            MCIERR_MISSING_STRING_ARGUMENT = (MCIERR_BASE + 13)
            MCIERR_BAD_INTEGER = (MCIERR_BASE + 14)
            MCIERR_PARSER_INTERNAL = (MCIERR_BASE + 15)
            MCIERR_DRIVER_INTERNAL = (MCIERR_BASE + 16)
            MCIERR_MISSING_PARAMETER = (MCIERR_BASE + 17)
            MCIERR_UNSUPPORTED_FUNCTION = (MCIERR_BASE + 18)
            MCIERR_FILE_NOT_FOUND = (MCIERR_BASE + 19)
            MCIERR_DEVICE_NOT_READY = (MCIERR_BASE + 20)
            MCIERR_INTERNAL = (MCIERR_BASE + 21)
            MCIERR_DRIVER = (MCIERR_BASE + 22)
            MCIERR_CANNOT_USE_ALL = (MCIERR_BASE + 23)
            MCIERR_MULTIPLE = (MCIERR_BASE + 24)
            MCIERR_EXTENSION_NOT_FOUND = (MCIERR_BASE + 25)
            MCIERR_OUTOFRANGE = (MCIERR_BASE + 26)
            MCIERR_FLAGS_NOT_COMPATIBLE = (MCIERR_BASE + 28)
            MCIERR_FILE_NOT_SAVED = (MCIERR_BASE + 30)
            MCIERR_DEVICE_TYPE_REQUIRED = (MCIERR_BASE + 31)
            MCIERR_DEVICE_LOCKED = (MCIERR_BASE + 32)
            MCIERR_DUPLICATE_ALIAS = (MCIERR_BASE + 33)
            MCIERR_BAD_CONSTANT = (MCIERR_BASE + 34)
            MCIERR_MUST_USE_SHAREABLE = (MCIERR_BASE + 35)
            MCIERR_MISSING_DEVICE_NAME = (MCIERR_BASE + 36)
            MCIERR_BAD_TIME_FORMAT = (MCIERR_BASE + 37)
            MCIERR_NO_CLOSING_QUOTE = (MCIERR_BASE + 38)
            MCIERR_DUPLICATE_FLAGS = (MCIERR_BASE + 39)
            MCIERR_INVALID_FILE = (MCIERR_BASE + 40)
            MCIERR_NULL_PARAMETER_BLOCK = (MCIERR_BASE + 41)
            MCIERR_UNNAMED_RESOURCE = (MCIERR_BASE + 42)
            MCIERR_NEW_REQUIRES_ALIAS = (MCIERR_BASE + 43)
            MCIERR_NOTIFY_ON_AUTO_OPEN = (MCIERR_BASE + 44)
            MCIERR_NO_ELEMENT_ALLOWED = (MCIERR_BASE + 45)
            MCIERR_NONAPPLICABLE_FUNCTION = (MCIERR_BASE + 46)
            MCIERR_ILLEGAL_FOR_AUTO_OPEN = (MCIERR_BASE + 47)
            MCIERR_FILENAME_REQUIRED = (MCIERR_BASE + 48)
            MCIERR_EXTRA_CHARACTERS = (MCIERR_BASE + 49)
            MCIERR_DEVICE_NOT_INSTALLED = (MCIERR_BASE + 50)
            MCIERR_GET_CD = (MCIERR_BASE + 51)
            MCIERR_SET_CD = (MCIERR_BASE + 52)
            MCIERR_SET_DRIVE = (MCIERR_BASE + 53)
            MCIERR_DEVICE_LENGTH = (MCIERR_BASE + 54)
            MCIERR_DEVICE_ORD_LENGTH = (MCIERR_BASE + 55)
            MCIERR_NO_INTEGER = (MCIERR_BASE + 56)
            MCIERR_WAVE_OUTPUTSINUSE = (MCIERR_BASE + 64)
            MCIERR_WAVE_SETOUTPUTINUSE = (MCIERR_BASE + 65)
            MCIERR_WAVE_INPUTSINUSE = (MCIERR_BASE + 66)
            MCIERR_WAVE_SETINPUTINUSE = (MCIERR_BASE + 67)
            MCIERR_WAVE_OUTPUTUNSPECIFIED = (MCIERR_BASE + 68)
            MCIERR_WAVE_INPUTUNSPECIFIED = (MCIERR_BASE + 69)
            MCIERR_WAVE_OUTPUTSUNSUITABLE = (MCIERR_BASE + 70)
            MCIERR_WAVE_SETOUTPUTUNSUITABLE = (MCIERR_BASE + 71)
            MCIERR_WAVE_INPUTSUNSUITABLE = (MCIERR_BASE + 72)
            MCIERR_WAVE_SETINPUTUNSUITABLE = (MCIERR_BASE + 73)
            MCIERR_SEQ_DIV_INCOMPATIBLE = (MCIERR_BASE + 80)
            MCIERR_SEQ_PORT_INUSE = (MCIERR_BASE + 81)
            MCIERR_SEQ_PORT_NONEXISTENT = (MCIERR_BASE + 82)
            MCIERR_SEQ_PORT_MAPNODEVICE = (MCIERR_BASE + 83)
            MCIERR_SEQ_PORT_MISCERROR = (MCIERR_BASE + 84)
            MCIERR_SEQ_TIMER = (MCIERR_BASE + 85)
            MCIERR_SEQ_PORTUNSPECIFIED = (MCIERR_BASE + 86)
            MCIERR_SEQ_NOMIDIPRESENT = (MCIERR_BASE + 87)
            MCIERR_NO_WINDOW = (MCIERR_BASE + 90)
            MCIERR_CREATEWINDOW = (MCIERR_BASE + 91)
            MCIERR_FILE_READ = (MCIERR_BASE + 92)
            MCIERR_FILE_WRITE = (MCIERR_BASE + 93)
            MCIERR_CUSTOM_DRIVER_BASE = (MCIERR_BASE + 256)
        End Enum

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="errorNumber"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal errorNumber As Integer)
            Me.ErrorNumber = errorNumber
        End Sub

        ''' <summary>
        ''' Returns the description for the value set in the ErrorNumber property.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private ReadOnly Property ErrorDescription() As String
            Get
                Select Case _errorNumber
                    Case MCIERR.MCIERR_BAD_CONSTANT
                        Return "Bad Constant"
                    Case MCIERR.MCIERR_BAD_INTEGER
                        Return "Bad Integer"
                    Case MCIERR.MCIERR_BAD_TIME_FORMAT
                        Return "Bad Time Format"
                    Case MCIERR.MCIERR_BASE
                        Return "Base error"
                    Case MCIERR.MCIERR_CANNOT_LOAD_DRIVER
                        Return "Cannot load driver"
                    Case MCIERR.MCIERR_CANNOT_USE_ALL
                        Return "Cannot use all"
                    Case MCIERR.MCIERR_CREATEWINDOW
                        Return "Create window"
                    Case MCIERR.MCIERR_CUSTOM_DRIVER_BASE
                        Return "Custom driver base"
                    Case MCIERR.MCIERR_DEVICE_LENGTH
                        Return "Device length"
                    Case MCIERR.MCIERR_DEVICE_LOCKED
                        Return "Device locked"
                    Case MCIERR.MCIERR_DEVICE_NOT_INSTALLED
                        Return "Device not installed"
                    Case MCIERR.MCIERR_DEVICE_NOT_READY
                        Return "Device not ready"
                    Case MCIERR.MCIERR_DEVICE_OPEN
                        Return "Device open"
                    Case MCIERR.MCIERR_DEVICE_ORD_LENGTH
                        Return "Device ORD length"
                    Case MCIERR.MCIERR_DEVICE_TYPE_REQUIRED
                        Return "Device type required"
                    Case MCIERR.MCIERR_DRIVER
                        Return "Driver"
                    Case MCIERR.MCIERR_DRIVER_INTERNAL
                        Return "Driver Internal"
                    Case MCIERR.MCIERR_DUPLICATE_ALIAS
                        Return "Duplicate alias"
                    Case MCIERR.MCIERR_DUPLICATE_FLAGS
                        Return "Duplicate flags"
                    Case MCIERR.MCIERR_EXTENSION_NOT_FOUND
                        Return "Extension not found"
                    Case MCIERR.MCIERR_EXTRA_CHARACTERS
                        Return "Extra characters"
                    Case MCIERR.MCIERR_FILE_NOT_FOUND
                        Return "File not found"
                    Case MCIERR.MCIERR_FILE_NOT_SAVED
                        Return "File not saved"
                    Case MCIERR.MCIERR_FILE_READ
                        Return "File read"
                    Case MCIERR.MCIERR_FILE_WRITE
                        Return "File write"
                    Case MCIERR.MCIERR_FILENAME_REQUIRED
                        Return "Filename required"
                    Case MCIERR.MCIERR_FLAGS_NOT_COMPATIBLE
                        Return "Flags not compatible"
                    Case MCIERR.MCIERR_GET_CD
                        Return "Get CD"
                    Case MCIERR.MCIERR_HARDWARE
                        Return "Hardware"
                    Case MCIERR.MCIERR_ILLEGAL_FOR_AUTO_OPEN
                        Return "Illegal for auto open"
                    Case MCIERR.MCIERR_INTERNAL
                        Return "Internal"
                    Case MCIERR.MCIERR_INVALID_DEVICE_ID
                        Return "Invalid Device ID"
                    Case MCIERR.MCIERR_INVALID_DEVICE_NAME
                        Return "Invalid Device Name"
                    Case MCIERR.MCIERR_INVALID_FILE
                        Return "Invalid file"
                    Case MCIERR.MCIERR_MISSING_COMMAND_STRING
                        Return "Missing command string"
                    Case MCIERR.MCIERR_MISSING_DEVICE_NAME
                        Return "Missing device name"
                    Case MCIERR.MCIERR_MISSING_PARAMETER
                        Return "Missing parameter"
                    Case MCIERR.MCIERR_MISSING_STRING_ARGUMENT
                        Return "Missing string argument"
                    Case MCIERR.MCIERR_MULTIPLE
                        Return "Multiple"
                    Case MCIERR.MCIERR_MUST_USE_SHAREABLE
                        Return "Must use shareable"
                    Case MCIERR.MCIERR_NEW_REQUIRES_ALIAS
                        Return "New requires alias"
                    Case MCIERR.MCIERR_NO_CLOSING_QUOTE
                        Return "No closing quote"
                    Case MCIERR.MCIERR_NO_ELEMENT_ALLOWED
                        Return "No element allowed"
                    Case MCIERR.MCIERR_NO_INTEGER
                        Return "No integer"
                    Case MCIERR.MCIERR_NO_WINDOW
                        Return "No window"
                    Case MCIERR.MCIERR_NONAPPLICABLE_FUNCTION
                        Return "No applicable function"
                    Case MCIERR.MCIERR_NOTIFY_ON_AUTO_OPEN
                        Return "Notify on auto open"
                    Case MCIERR.MCIERR_NULL_PARAMETER_BLOCK
                        Return "Null parameter block"
                    Case MCIERR.MCIERR_OUT_OF_MEMORY
                        Return "Out of memory"
                    Case MCIERR.MCIERR_OUTOFRANGE
                        Return "Out of range"
                    Case MCIERR.MCIERR_PARAM_OVERFLOW
                        Return "Param overflow"
                    Case MCIERR.MCIERR_PARSER_INTERNAL
                        Return "Parser internal"
                    Case MCIERR.MCIERR_SEQ_DIV_INCOMPATIBLE
                        Return "Seq div incompatible"
                    Case MCIERR.MCIERR_SEQ_NOMIDIPRESENT
                        Return "Seq nomid present"
                    Case MCIERR.MCIERR_SEQ_PORT_INUSE
                        Return "Seq Port in use"
                    Case MCIERR.MCIERR_SEQ_PORT_MAPNODEVICE
                        Return "Seq Port Map No Device"
                    Case MCIERR.MCIERR_SEQ_PORT_MISCERROR
                        Return "Seq Port Misc Error"
                    Case MCIERR.MCIERR_SEQ_PORT_NONEXISTENT
                        Return "Seq Port Nonexistent"
                    Case MCIERR.MCIERR_SEQ_PORTUNSPECIFIED
                        Return "Seq Port Unspecified"
                    Case MCIERR.MCIERR_SEQ_TIMER
                        Return "Seq Timer"
                    Case MCIERR.MCIERR_SET_CD
                        Return "Set CD"
                    Case MCIERR.MCIERR_SET_DRIVE
                        Return "Set Drive"
                    Case MCIERR.MCIERR_UNNAMED_RESOURCE
                        Return "Unnamed Resource"
                    Case MCIERR.MCIERR_UNRECOGNIZED_COMMAND
                        Return "Unrecognized Command"
                    Case MCIERR.MCIERR_UNRECOGNIZED_KEYWORD
                        Return "Unrecognized Keyword"
                    Case MCIERR.MCIERR_UNSUPPORTED_FUNCTION
                        Return "Unsupported Function"
                    Case MCIERR.MCIERR_WAVE_INPUTSINUSE
                        Return "Wave input in use"
                    Case MCIERR.MCIERR_WAVE_INPUTSUNSUITABLE
                        Return "Wave inputs unsuitable"
                    Case MCIERR.MCIERR_WAVE_INPUTUNSPECIFIED
                        Return "Wave inputs unspecified"
                    Case MCIERR.MCIERR_WAVE_OUTPUTSINUSE
                        Return "Wave outputs in use"
                    Case MCIERR.MCIERR_WAVE_OUTPUTSUNSUITABLE
                        Return "Wave outputs unsuitable"
                    Case MCIERR.MCIERR_WAVE_OUTPUTUNSPECIFIED
                        Return "Wave outputs unspecified"
                    Case MCIERR.MCIERR_WAVE_SETINPUTINUSE
                        Return "Wave set input in use"
                    Case MCIERR.MCIERR_WAVE_SETINPUTUNSUITABLE
                        Return "Wave set input unsuitable"
                    Case MCIERR.MCIERR_WAVE_SETOUTPUTINUSE
                        Return "Wave set output in use"
                    Case MCIERR.MCIERR_WAVE_SETOUTPUTUNSUITABLE
                        Return "Wave set output unsuitable"
                    Case Else
                        Return "Undefined"
                End Select
            End Get
        End Property

        Private _errorNumber As Integer
        ''' <summary>
        ''' The error number that the mciSendString API returned.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ErrorNumber() As Integer
            Get
                Return _errorNumber
            End Get
            Set(ByVal value As Integer)
                _errorNumber = value
            End Set
        End Property

    End Class

End Namespace