using System.ComponentModel;

namespace Tgs.GameInterface.SkywindService.Core;

public enum SkywindErrorCode
{
    [Description("Access Token is missing")]
    ACCESS_TOKEN_IS_MISSING = 10,

    [Description("Validate Error")]
    VALIDATE_ERROR = 40,

    [Description("Could not find entity")]
    COULD_NOT_FIND_ENTITY = 51,

    [Description("One of the parents is suspended")]
    ONE_OF_THE_PARENTS_IS_SUSPENDED = 62,

    [Description("Country not in list")]
    COUNTRY_NOT_IN_LIST = 82,

    [Description("Currency not found")]
    CURRENCY_NOT_FOUND = 85,

    [Description("Currency not in list")]
    CURRENCY_NOT_IN_LIST = 87,

    [Description("Player does not have sufficient balance to perform an operation")]
    PLAYER_DOES_NOT_HAVE_SUFFICIENT_BALANCE_TO_PERFORM_AN_OPERATION = 91,

    [Description("Language not in list")]
    LANGUAGE_NOT_IN_LIST = 95,

    [Description("Player’s code already exists")]
    PLAYERS_CODE_ALREADY_EXISTS = 100,

    [Description("Not a brand")]
    NOT_A_BRAND = 101,

    [Description("Player not found")]
    PLAYER_NOT_FOUND = 102,

    [Description("Max capacity of transaction is reached")]
    MAX_CAPACITY_OF_TRANSACTION_IS_REACHED = 108,

    [Description("Access token error")]
    ACCESS_TOKEN_ERROR = 204,

    [Description("Access Token is expired")]
    ACCESS_TOKEN_IS_EXPIRED = 205,

    [Description("User doesn’t have permission to execute the operation")]
    USER_DOESNT_HAVE_PERMISSION_TO_EXECUTE_THE_OPERATION = 206,

    [Description("Game group is not found")]
    GAME_GROUP_IS_NOT_FOUND = 211,

    [Description("Login Failed, please contact Skywind Support to resolve the issue")]
    LOGIN_FAILED_PLEASE_CONTACT_SKYWIND_SUPPORT_TO_RESOLVE_THE_ISSUE = 230,

    [Description("Merchant brand doesn’t support this operation")]
    MERCHANT_BRAND_DOESNT_SUPPORT_THIS_OPERATION = 503,

    [Description("Brand has exceeded max number of test players")]
    BRAND_HAS_EXCEEDED_MAX_NUMBER_OF_TEST_PLAYERS = 707,

    [Description("TWO FACTOR AUTHENTICATION IS NOT SET FOR USER")]
    TWO_FACTOR_AUTHENTICATION_IS_NOT_SET_FOR_USER = 715,

    [Description("An error occurred when sending sms")]
    AN_ERROR_OCCURRED_WHEN_SENDING_SMS = 719,

    [Description("An error occurred when sending email")]
    AN_ERROR_OCCURRED_WHEN_SENDING_EMAIL = 720,

    [Description("Two factor auth code has been sent recently. Repeat attempt a little later")]
    TWO_FACTOR_AUTH_CODE_HAS_BEEN_SENT_RECENTLY_REPEAT_ATTEMPT_A_LITTLE_LATER = 727,

    [Description("Entity does not have sufficient balance to perform an operation")]
    ENTITY_DOES_NOT_HAVE_SUFFICIENT_BALANCE_TO_PERFORM_AN_OPERATION = 729,

    [Description("Entity is under maintenance")]
    ENTITY_IS_UNDER_MAINTENANCE = 735,

    [Description("Player default currency doesn't match with requested")]
    PLAYER_DEFAULT_CURRENCY_DOESNT_MATCH_WITH_REQUESTED = 755,

    [Description("Access Session is expired")]
    ACCESS_SESSION_IS_EXPIRED = 792,

    [Description("Login Failed")]
    LOGIN_FAILED = 998,
}
