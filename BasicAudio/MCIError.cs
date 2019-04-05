namespace BasicAudio
{
    /// <summary>
    /// This is a lookup class to identify the MCI errors that may occur.
    /// </summary>
    /// <remarks>
    /// There's is an API that will also do this for us, We should consider using that in the future.
    /// </remarks>
    public class MCIError
    {
        //*********************************************************************************************************************
        //
        //             Class:  MCIError
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  03/31/2007
        //      Last Updated:  04/09/2016
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// The device error enums
        /// </summary>
        /// <remarks></remarks>
        public enum MCIERR
        {
            MCIERR_BASE = 256,
            MCIERR_INVALID_DEVICE_ID = (MCIERR_BASE + 1),
            MCIERR_UNRECOGNIZED_KEYWORD = (MCIERR_BASE + 3),
            MCIERR_UNRECOGNIZED_COMMAND = (MCIERR_BASE + 5),
            MCIERR_HARDWARE = (MCIERR_BASE + 6),
            MCIERR_INVALID_DEVICE_NAME = (MCIERR_BASE + 7),
            MCIERR_OUT_OF_MEMORY = (MCIERR_BASE + 8),
            MCIERR_DEVICE_OPEN = (MCIERR_BASE + 9),
            MCIERR_CANNOT_LOAD_DRIVER = (MCIERR_BASE + 10),
            MCIERR_MISSING_COMMAND_STRING = (MCIERR_BASE + 11),
            MCIERR_PARAM_OVERFLOW = (MCIERR_BASE + 12),
            MCIERR_MISSING_STRING_ARGUMENT = (MCIERR_BASE + 13),
            MCIERR_BAD_INTEGER = (MCIERR_BASE + 14),
            MCIERR_PARSER_INTERNAL = (MCIERR_BASE + 15),
            MCIERR_DRIVER_INTERNAL = (MCIERR_BASE + 16),
            MCIERR_MISSING_PARAMETER = (MCIERR_BASE + 17),
            MCIERR_UNSUPPORTED_FUNCTION = (MCIERR_BASE + 18),
            MCIERR_FILE_NOT_FOUND = (MCIERR_BASE + 19),
            MCIERR_DEVICE_NOT_READY = (MCIERR_BASE + 20),
            MCIERR_INTERNAL = (MCIERR_BASE + 21),
            MCIERR_DRIVER = (MCIERR_BASE + 22),
            MCIERR_CANNOT_USE_ALL = (MCIERR_BASE + 23),
            MCIERR_MULTIPLE = (MCIERR_BASE + 24),
            MCIERR_EXTENSION_NOT_FOUND = (MCIERR_BASE + 25),
            MCIERR_OUTOFRANGE = (MCIERR_BASE + 26),
            MCIERR_FLAGS_NOT_COMPATIBLE = (MCIERR_BASE + 28),
            MCIERR_FILE_NOT_SAVED = (MCIERR_BASE + 30),
            MCIERR_DEVICE_TYPE_REQUIRED = (MCIERR_BASE + 31),
            MCIERR_DEVICE_LOCKED = (MCIERR_BASE + 32),
            MCIERR_DUPLICATE_ALIAS = (MCIERR_BASE + 33),
            MCIERR_BAD_CONSTANT = (MCIERR_BASE + 34),
            MCIERR_MUST_USE_SHAREABLE = (MCIERR_BASE + 35),
            MCIERR_MISSING_DEVICE_NAME = (MCIERR_BASE + 36),
            MCIERR_BAD_TIME_FORMAT = (MCIERR_BASE + 37),
            MCIERR_NO_CLOSING_QUOTE = (MCIERR_BASE + 38),
            MCIERR_DUPLICATE_FLAGS = (MCIERR_BASE + 39),
            MCIERR_INVALID_FILE = (MCIERR_BASE + 40),
            MCIERR_NULL_PARAMETER_BLOCK = (MCIERR_BASE + 41),
            MCIERR_UNNAMED_RESOURCE = (MCIERR_BASE + 42),
            MCIERR_NEW_REQUIRES_ALIAS = (MCIERR_BASE + 43),
            MCIERR_NOTIFY_ON_AUTO_OPEN = (MCIERR_BASE + 44),
            MCIERR_NO_ELEMENT_ALLOWED = (MCIERR_BASE + 45),
            MCIERR_NONAPPLICABLE_FUNCTION = (MCIERR_BASE + 46),
            MCIERR_ILLEGAL_FOR_AUTO_OPEN = (MCIERR_BASE + 47),
            MCIERR_FILENAME_REQUIRED = (MCIERR_BASE + 48),
            MCIERR_EXTRA_CHARACTERS = (MCIERR_BASE + 49),
            MCIERR_DEVICE_NOT_INSTALLED = (MCIERR_BASE + 50),
            MCIERR_GET_CD = (MCIERR_BASE + 51),
            MCIERR_SET_CD = (MCIERR_BASE + 52),
            MCIERR_SET_DRIVE = (MCIERR_BASE + 53),
            MCIERR_DEVICE_LENGTH = (MCIERR_BASE + 54),
            MCIERR_DEVICE_ORD_LENGTH = (MCIERR_BASE + 55),
            MCIERR_NO_INTEGER = (MCIERR_BASE + 56),
            MCIERR_WAVE_OUTPUTSINUSE = (MCIERR_BASE + 64),
            MCIERR_WAVE_SETOUTPUTINUSE = (MCIERR_BASE + 65),
            MCIERR_WAVE_INPUTSINUSE = (MCIERR_BASE + 66),
            MCIERR_WAVE_SETINPUTINUSE = (MCIERR_BASE + 67),
            MCIERR_WAVE_OUTPUTUNSPECIFIED = (MCIERR_BASE + 68),
            MCIERR_WAVE_INPUTUNSPECIFIED = (MCIERR_BASE + 69),
            MCIERR_WAVE_OUTPUTSUNSUITABLE = (MCIERR_BASE + 70),
            MCIERR_WAVE_SETOUTPUTUNSUITABLE = (MCIERR_BASE + 71),
            MCIERR_WAVE_INPUTSUNSUITABLE = (MCIERR_BASE + 72),
            MCIERR_WAVE_SETINPUTUNSUITABLE = (MCIERR_BASE + 73),
            MCIERR_SEQ_DIV_INCOMPATIBLE = (MCIERR_BASE + 80),
            MCIERR_SEQ_PORT_INUSE = (MCIERR_BASE + 81),
            MCIERR_SEQ_PORT_NONEXISTENT = (MCIERR_BASE + 82),
            MCIERR_SEQ_PORT_MAPNODEVICE = (MCIERR_BASE + 83),
            MCIERR_SEQ_PORT_MISCERROR = (MCIERR_BASE + 84),
            MCIERR_SEQ_TIMER = (MCIERR_BASE + 85),
            MCIERR_SEQ_PORTUNSPECIFIED = (MCIERR_BASE + 86),
            MCIERR_SEQ_NOMIDIPRESENT = (MCIERR_BASE + 87),
            MCIERR_NO_WINDOW = (MCIERR_BASE + 90),
            MCIERR_CREATEWINDOW = (MCIERR_BASE + 91),
            MCIERR_FILE_READ = (MCIERR_BASE + 92),
            MCIERR_FILE_WRITE = (MCIERR_BASE + 93),
            MCIERR_CUSTOM_DRIVER_BASE = (MCIERR_BASE + 256)
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="errorNumber"></param>
        /// <remarks></remarks>
        public MCIError(int errorNumber)
        {
            this.ErrorNumber = errorNumber;
        }

        /// <summary>
        /// Returns the description for the value set in the ErrorNumber property.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        private string ErrorDescription
        {
            get
            {
                switch (_errorNumber)
                {
                    case (int)MCIERR.MCIERR_BAD_CONSTANT:
                        return "Bad Constant";
                    case (int)MCIERR.MCIERR_BAD_INTEGER:
                        return "Bad Integer";
                    case (int)MCIERR.MCIERR_BAD_TIME_FORMAT:
                        return "Bad Time Format";
                    case (int)MCIERR.MCIERR_BASE:
                        return "Base error";
                    case (int)MCIERR.MCIERR_CANNOT_LOAD_DRIVER:
                        return "Cannot load driver";
                    case (int)MCIERR.MCIERR_CANNOT_USE_ALL:
                        return "Cannot use all";
                    case (int)MCIERR.MCIERR_CREATEWINDOW:
                        return "Create window";
                    case (int)MCIERR.MCIERR_CUSTOM_DRIVER_BASE:
                        return "Custom driver base";
                    case (int)MCIERR.MCIERR_DEVICE_LENGTH:
                        return "Device length";
                    case (int)MCIERR.MCIERR_DEVICE_LOCKED:
                        return "Device locked";
                    case (int)MCIERR.MCIERR_DEVICE_NOT_INSTALLED:
                        return "Device not installed";
                    case (int)MCIERR.MCIERR_DEVICE_NOT_READY:
                        return "Device not ready";
                    case (int)MCIERR.MCIERR_DEVICE_OPEN:
                        return "Device open";
                    case (int)MCIERR.MCIERR_DEVICE_ORD_LENGTH:
                        return "Device ORD length";
                    case (int)MCIERR.MCIERR_DEVICE_TYPE_REQUIRED:
                        return "Device type required";
                    case (int)MCIERR.MCIERR_DRIVER:
                        return "Driver";
                    case (int)MCIERR.MCIERR_DRIVER_INTERNAL:
                        return "Driver Internal";
                    case (int)MCIERR.MCIERR_DUPLICATE_ALIAS:
                        return "Duplicate alias";
                    case (int)MCIERR.MCIERR_DUPLICATE_FLAGS:
                        return "Duplicate flags";
                    case (int)MCIERR.MCIERR_EXTENSION_NOT_FOUND:
                        return "Extension not found";
                    case (int)MCIERR.MCIERR_EXTRA_CHARACTERS:
                        return "Extra characters";
                    case (int)MCIERR.MCIERR_FILE_NOT_FOUND:
                        return "File not found";
                    case (int)MCIERR.MCIERR_FILE_NOT_SAVED:
                        return "File not saved";
                    case (int)MCIERR.MCIERR_FILE_READ:
                        return "File read";
                    case (int)MCIERR.MCIERR_FILE_WRITE:
                        return "File write";
                    case (int)MCIERR.MCIERR_FILENAME_REQUIRED:
                        return "Filename required";
                    case (int)MCIERR.MCIERR_FLAGS_NOT_COMPATIBLE:
                        return "Flags not compatible";
                    case (int)MCIERR.MCIERR_GET_CD:
                        return "Get CD";
                    case (int)MCIERR.MCIERR_HARDWARE:
                        return "Hardware";
                    case (int)MCIERR.MCIERR_ILLEGAL_FOR_AUTO_OPEN:
                        return "Illegal for auto open";
                    case (int)MCIERR.MCIERR_INTERNAL:
                        return "Internal";
                    case (int)MCIERR.MCIERR_INVALID_DEVICE_ID:
                        return "Invalid Device ID";
                    case (int)MCIERR.MCIERR_INVALID_DEVICE_NAME:
                        return "Invalid Device Name";
                    case (int)MCIERR.MCIERR_INVALID_FILE:
                        return "Invalid file";
                    case (int)MCIERR.MCIERR_MISSING_COMMAND_STRING:
                        return "Missing command string";
                    case (int)MCIERR.MCIERR_MISSING_DEVICE_NAME:
                        return "Missing device name";
                    case (int)MCIERR.MCIERR_MISSING_PARAMETER:
                        return "Missing parameter";
                    case (int)MCIERR.MCIERR_MISSING_STRING_ARGUMENT:
                        return "Missing string argument";
                    case (int)MCIERR.MCIERR_MULTIPLE:
                        return "Multiple";
                    case (int)MCIERR.MCIERR_MUST_USE_SHAREABLE:
                        return "Must use shareable";
                    case (int)MCIERR.MCIERR_NEW_REQUIRES_ALIAS:
                        return "New requires alias";
                    case (int)MCIERR.MCIERR_NO_CLOSING_QUOTE:
                        return "No closing quote";
                    case (int)MCIERR.MCIERR_NO_ELEMENT_ALLOWED:
                        return "No element allowed";
                    case (int)MCIERR.MCIERR_NO_INTEGER:
                        return "No integer";
                    case (int)MCIERR.MCIERR_NO_WINDOW:
                        return "No window";
                    case (int)MCIERR.MCIERR_NONAPPLICABLE_FUNCTION:
                        return "No applicable function";
                    case (int)MCIERR.MCIERR_NOTIFY_ON_AUTO_OPEN:
                        return "Notify on auto open";
                    case (int)MCIERR.MCIERR_NULL_PARAMETER_BLOCK:
                        return "Null parameter block";
                    case (int)MCIERR.MCIERR_OUT_OF_MEMORY:
                        return "Out of memory";
                    case (int)MCIERR.MCIERR_OUTOFRANGE:
                        return "Out of range";
                    case (int)MCIERR.MCIERR_PARAM_OVERFLOW:
                        return "Param overflow";
                    case (int)MCIERR.MCIERR_PARSER_INTERNAL:
                        return "Parser internal";
                    case (int)MCIERR.MCIERR_SEQ_DIV_INCOMPATIBLE:
                        return "Seq div incompatible";
                    case (int)MCIERR.MCIERR_SEQ_NOMIDIPRESENT:
                        return "Seq nomid present";
                    case (int)MCIERR.MCIERR_SEQ_PORT_INUSE:
                        return "Seq Port in use";
                    case (int)MCIERR.MCIERR_SEQ_PORT_MAPNODEVICE:
                        return "Seq Port Map No Device";
                    case (int)MCIERR.MCIERR_SEQ_PORT_MISCERROR:
                        return "Seq Port Misc Error";
                    case (int)MCIERR.MCIERR_SEQ_PORT_NONEXISTENT:
                        return "Seq Port Nonexistent";
                    case (int)MCIERR.MCIERR_SEQ_PORTUNSPECIFIED:
                        return "Seq Port Unspecified";
                    case (int)MCIERR.MCIERR_SEQ_TIMER:
                        return "Seq Timer";
                    case (int)MCIERR.MCIERR_SET_CD:
                        return "Set CD";
                    case (int)MCIERR.MCIERR_SET_DRIVE:
                        return "Set Drive";
                    case (int)MCIERR.MCIERR_UNNAMED_RESOURCE:
                        return "Unnamed Resource";
                    case (int)MCIERR.MCIERR_UNRECOGNIZED_COMMAND:
                        return "Unrecognized Command";
                    case (int)MCIERR.MCIERR_UNRECOGNIZED_KEYWORD:
                        return "Unrecognized Keyword";
                    case (int)MCIERR.MCIERR_UNSUPPORTED_FUNCTION:
                        return "Unsupported Function";
                    case (int)MCIERR.MCIERR_WAVE_INPUTSINUSE:
                        return "Wave input in use";
                    case (int)MCIERR.MCIERR_WAVE_INPUTSUNSUITABLE:
                        return "Wave inputs unsuitable";
                    case (int)MCIERR.MCIERR_WAVE_INPUTUNSPECIFIED:
                        return "Wave inputs unspecified";
                    case (int)MCIERR.MCIERR_WAVE_OUTPUTSINUSE:
                        return "Wave outputs in use";
                    case (int)MCIERR.MCIERR_WAVE_OUTPUTSUNSUITABLE:
                        return "Wave outputs unsuitable";
                    case (int)MCIERR.MCIERR_WAVE_OUTPUTUNSPECIFIED:
                        return "Wave outputs unspecified";
                    case (int)MCIERR.MCIERR_WAVE_SETINPUTINUSE:
                        return "Wave set input in use";
                    case (int)MCIERR.MCIERR_WAVE_SETINPUTUNSUITABLE:
                        return "Wave set input unsuitable";
                    case (int)MCIERR.MCIERR_WAVE_SETOUTPUTINUSE:
                        return "Wave set output in use";
                    case (int)MCIERR.MCIERR_WAVE_SETOUTPUTUNSUITABLE:
                        return "Wave set output unsuitable";
                    default:
                        return "Undefined";
                }
            }
        }

        private int _errorNumber;
        /// <summary>
        /// The error number that the mciSendString API returned.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int ErrorNumber
        {
            get { return _errorNumber; }
            set { _errorNumber = value; }
        }

    }

}
