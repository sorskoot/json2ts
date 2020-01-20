var TimmyTools;
(function (TimmyTools) {
    /**
      * Checks if a piece of code is valid JSON
      * @param code {String} string containing a piece of JSON code
      */
    function IsValidJson(code) {
        return !(/[^,:{}\[\]0-9.\-+Eaeflnr-u \n\r\t]/.test(code.replace(/"(\\.|[^"\\])*"/g, ''))) &&
            eval('(' + code + ')');
    }
    TimmyTools.IsValidJson = IsValidJson;
})(TimmyTools || (TimmyTools = {}));
//# sourceMappingURL=timmytools.js.map