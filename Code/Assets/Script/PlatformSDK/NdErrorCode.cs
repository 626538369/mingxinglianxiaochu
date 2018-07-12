using UnityEngine;
using System.Collections;

public class NdErrorCode
{
	/**
	 * Copy from Nd91Android sdk NdCommplatform
	 * **/
	// Field descriptor #346 I
	public const int LOGOUT_TO_RESET_AUTO_LOGIN_CONFIG = 1;
	
	// Field descriptor #346 I
	public const int LOGOUT_TO_NON_RESET_AUTO_LOGIN_CONFIG = 0;
	
	// Field descriptor #346 I
	public const int GET_USER_BASE_INFO = 1;
	
	// Field descriptor #346 I
	public const int GET_USER_POINT = 2;
	
	// Field descriptor #346 I
	public const int GET_USER_EMOTION = 4;
	
	// Field descriptor #346 I
	public const int SCREEN_ORIENTATION_PORTRAIT = 0;
	
	// Field descriptor #346 I
	public const int SCREEN_ORIENTATION_LANDSCAPE = 1;
	
	// Field descriptor #346 I
	public const int SCREEN_ORIENTATION_AUTO = 2;
	
	// Field descriptor #346 I
	public const int UPDATESTATUS_NONE = 0;
	
	// Field descriptor #346 I
	public const int UPDATESTATUS_UNMOUNTED_SDCARD = 1;
	
	// Field descriptor #346 I (deprecated)
	public const int UPDATESTATUS_CANCEL_ENFORCE_UPDATE = 2;
	
	// Field descriptor #346 I
	public const int UPDATESTATUS_CANCEL_UPDATE = 3;
	
	// Field descriptor #346 I
	public const int UPDATESTATUS_CHECK_FAILURE = 4;
	
	// Field descriptor #346 I
	public const int UPDATESTATUS_FORCES_LOADING = 5;
	
	// Field descriptor #346 I
	public const int UPDATESTATUS_RECOMMEND_LOADING = 6;
	
	// Field descriptor #346 I
	public const int DEFAULT_ICON_TYPE_HEAD = 1;
	
	// Field descriptor #346 I
	public const int DEFAULT_ICON_TYPE_APP = 2;
	
	// Field descriptor #346 I
	public const int SCORE_SUBMIT_SUCCESS = 0;
	
	// Field descriptor #346 I
	public const int SCORE_SAVE_LOCAL = 1;
	
	// Field descriptor #346 I
	public const int LEADERBOARD_NOT_EXIST = 2;
	
	
	/**
	 * Copy from Nd91Android sdk NdErrorCode
	 * **/
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_SUCCESS = 0;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_UNKNOWN = -1;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_NETWORK_FAIL = -2;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_PACKAGE_INVALID = -3;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_SESSIONID_INVALID = -4;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_PARAM = -5;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_CLIENT_APP_ID_INVALID = -6;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_NETWORK_ERROR = -7;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_APP_KEY_INVALID = -8;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_NO_SIM = -9;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_SERVER_RETURN_ERROR = -10;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_HAS_NOT_LOGIN = -11;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_CANCEL = -12;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_UNCHECK = -13;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_UNPERMISSTION = -15;
	
	// Field descriptor #146 I
	public const int ND_COM_GUEST_OFFICIAL_SUCCESS = -31;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_USER_SWITCH_ACCOUNT = -50;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_USER_RESTART = -51;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_ACCOUNT_INVALID = -100;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_PASSWORD_INVALID = -101;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_LOGIN_FAIL = -102;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_ACCOUNT_NOT_EXIST = -103;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_ACCOUNT_PASSWORD_ERROR = -104;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_TOO_MUCH_ACCOUNT_REGISTED = -105;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_REGIST_FAIL = -106;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_ACCOUNT_HAS_EXIST = -107;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_VERIFY_ACCOUNT_FAIL = -108;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_PARAMETER_INVALID = -109;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_IGNORE_UPLOAD = -110;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_PARAMETER_INVALID_ACCOUNT = -113;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_PARAMETER_TOKEN_INVALID = -114;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_NICKNAME_INVALID = -201;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_NEW_PASSWORD_INVALID = -301;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_OLD_PASSWORD_INVALID = -302;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_OLD_PASSWORD_ERROR = -303;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_HAS_SET_PHONE_NUM = -401;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_PHONE_HAS_REGISTED = -402;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_PHONE_SEND_REPEATED = -403;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_PHONE_VERIFY_CODE_INVALID = -404;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_TRUE_NAME_INVALID = -501;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_EMOTION_LENGTH_TOO_LONG = -601;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_EMOTION_CONTENT_INVALID = -602;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_PERMISSION_NOT_ENOUGH = -701;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_IMAGE_SIZE_TOO_LARGE = -801;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_IMAGE_DATA_INVALID = -802;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_PHOTO_NOT_CHANGED = -1001;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_NO_CUSTOM_PHOTO = -1002;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_APP_NOT_EXIST = -2001;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_ICON_NOT_CHANGED = -2002;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_NO_CUSTOM_APP_ICON = -2003;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_ICON_NO_EXIST = -2004;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_PAY_PASSWORD_ERROR = -3001;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_PAY_ACCOUNT_NOT_ACTIVED = -3002;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_PAY_PASSWORD_NOT_SET = -3003;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_PAY_PASSWORD_NOT_VERIFY = -4001;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_BALANCE_NOT_ENOUGH = -4002;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_ORDER_SERIAL_DUPLICATE = -4003;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_PAY_REQUEST_SUBMITTED = -4004;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_PAGE_REQUIRED_NOT_VALID = -5001;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_RECHARGE_MONEY_INVALID = -6001;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_SMS_RECHARGE_ACCOUNT_INVALID = -6002;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_NO_PHONE_NUM = -6003;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_PAY_ASYN_SMS_SENT = -6004;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_RECHARGE_CARD_NUMBER_ERROR = -7001;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_RECHARGE_CARD_PASSWORD_ERROR = -7002;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_RECHARGE_CARD_TYPE_NOT_SUPPORT = -7003;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_USER_NOT_EXIST = -10011;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_FRIEND_NOT_EXIST = -10012;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_ALREADY_BE_YOUR_FRIEND = -11002;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_NOTE_LENGTH_INVALID = -11003;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_ARRIVE_MAX_FRIEND_NUM = -11004;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_APP_ID_INVALID = -13001;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_ACTIVITY_TYPE_INVALID = -13002;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_MSG_NOT_EXIST = -14001;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_CONTENT_LENGTH_INVALID = -15001;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_NOT_ALLOWED_TO_SEND_MSG = -15002;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_CAN_NOT_SEND_MSG_TO_SELF = -15003;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_CLIENT_TAG = -16001;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_INVALID_COMMAND_TAG = -16002;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_INVALID_CONTENT_TAG = -16003;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_CUSTOM_TAG_ARG_NOT_ENOUGH = -16004;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_CUSTOM_TAG_ARG_NOT_INVALID = -16005;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_FEEDBACK_ID_INVALID = -17001;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_TOO_MUCH_ACCOUNT_LOGINED = -17002;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_TEMPLATE_ID_INVALID = -18001;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_TEMPLATE_PARAMLIST_ERROR = -18002;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_PAY_FAILURE = -18003;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_PAY_CANCEL = -18004;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_LEADER_BOARD_NO_EXIST = -19001;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_USER_LEADER_BOARD_NO_EXIST = -19002;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_NO_PLAYING_FRIEND = -19003;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_ACHIEVEMENT_NO_EXIST = -19004;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_USER_NO_BIND_3RD_ACCOUNT = -19030;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_CONTENT_SHARE_REPEATED = -19031;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_UNEXIST_ORDER = -19032;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_PAY_FOR_ANOTHER_NOT_CURRENT_USER = -19033;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_NOT_FRIEND = -19034;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_3RD_ACCOUNT_NO_FRIEND = -19040;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_ALREADY_BIND_3RD_ACCOUNT = -19041;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_3RD_ACCOUNT_HAS_BIND_91ACCOUNT = -19042;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_ALREADY_BIND_91ACCOUNT = -19043;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_3RD_ACCOUNT_LOGIN_INFO_LOSED = -19044;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_CANNOT_VERIFY_3RD_ACCOUNT_PASSWORD = -19045;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_3RD_ACCOUNT_BIND_91ACCOUNT_EXECEPTION = -19046;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_INVALID_PRODUCT_CATE = -21001;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_INVALID_FEE_TYPE = -21002;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_INFO_UNEXISTS = -22001;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_CAN_NOT_UNBIND = -22002;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_NOT_MATCH_ACCOUNT = -22003;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_RESEND = -23001;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_TIME_OUT = -23002;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_USE_INVIALD = -23003;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_GOODS_ID_INVALID = -23004;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_QUERY_BALANCE_FAIL = -24001;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_REQUEST_SERIAL_FAIL = -24002;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_EXIT_FROM_RECHARGE = -24003;
	
	// Field descriptor #146 I
	public const int ND_COM_PLATFORM_ERROR_SDK_VALID = -14;
	
	// Field descriptor #146 I
	public const int ND_COM_PHONENO_INVALID = -25001;
	
	// Field descriptor #146 I
	public const int ND_COM_REBIND = -25002;
	
	// Field descriptor #146 I
	public const int ND_COM_HAS_BIND = -25003;
	
	// Field descriptor #146 I
	public const int ND_COM_UNBIND_PHONE = -25004;
	
	// Field descriptor #146 I
	public const int ND_COM_PHONENO_INCONSISTENT = -25005;
	
	// Field descriptor #146 I
	public const int ND_COM_SMSCODE_ERROR = -25006;
	
	// Field descriptor #146 I
	public const int ND_COM_SMSCODE_EXPIRED = -25007;
	
	// Field descriptor #146 I
	public const int ND_COM_NOT_VERIFIED = -25008;
	
	// Field descriptor #146 I
	public const int ND_COM_UN_CONDITION = -25009;
	
	// Field descriptor #146 I
	public const int ND_COM_LOTTREYED = -25010;
	
	// Field descriptor #146 I
	public const int ND_COM_SEND_TOO_MORE = -25011;
	
	// Field descriptor #146 I
	public const int ND_COM_VIP_CANT_FIND = -25012;
	
	// Field descriptor #146 I
	public const int ND_COM_INCONSISTENT_BRFORE = -25013;
	
	// Field descriptor #146 I
	public const int ND_COM_INCONSISTENT_CONTLOGINBYGUEST = -26001;
	
	// Field descriptor #146 I
	public const int ND_COM_INCONSISTENT_UNNEET_OFFICIAL = -26002;
	
	// Field descriptor #146 I
	public const int ND_COM_INCONSISTENT_UIN_UNVALID = -26003;
	
	// Field descriptor #146 I
	public const int ND_COM_APP_PROMOTION_START_PROMOTION_FREQUENCY = -27001;
	
	// Field descriptor #146 I
	public const int ND_COM_APP_PROMOTION_NO_START_PROMOTION = -27002;
	
	// Field descriptor #146 I
	public const int ND_COM_APP_PROMOTION_CHECK_OAUTH_FAIL = -28001;
	
	// Field descriptor #146 I
	public const int ND_COM_APP_PROMOTION_CHECK_THIRD_BIND_FAIL = -28002;
}
