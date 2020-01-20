module TimmyTools {

/**
  * Checks if a piece of code is valid JSON
  * @param code {String} string containing a piece of JSON code
  */
    export function IsValidJson(code: string): boolean {
        return !(/[^,:{}\[\]0-9.\-+Eaeflnr-u \n\r\t]/.test(
            code.replace(/"(\\.|[^"\\])*"/g, ''))) &&
            eval('(' + code + ')');
    }


}