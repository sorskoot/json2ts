/// <reference path="typings/knockout/knockout.d.ts" />
/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="timmytools.ts" />
var json2ts;
(function (json2ts) {
    var Json2tsViewModel = /** @class */ (function () {
        function Json2tsViewModel() {
            var _this = this;
            this.code = ko.observable();
            this.modulename = ko.observable("someModule");
            this.rootObject = ko.observable("root");
            this.error = ko.observable();
            this.inputErrorMessage = ko.observable();
            this.resultcode = ko.observable();
            this.getTS = function () {
                if (!_this.code()) {
                    _this.inputErrorMessage("please enter JSON code or an URL to service that returns JSON");
                    return;
                }
                _this.callServer(_this.code());
            };
        }
        Json2tsViewModel.prototype.callServer = function (code) {
            var _this = this;
            if (!TimmyTools.IsValidJson(code) && code.substr(0, 4) != 'http') {
                this.inputErrorMessage("There seems to be an error in the JSON");
                return;
            }
            this.inputErrorMessage(null);
            code = code.replace(/\</g, "");
            $.post("/Home/GetTypeScriptDefinition", { code: code, ns: this.modulename(), root: this.rootObject() }, function (result) {
                if (!result.error) {
                    _this.error(null);
                    _this.resultcode(result);
                }
                else {
                    _this.error(result.error);
                }
            });
        };
        return Json2tsViewModel;
    }());
    $(document).ready(function () {
        $('#help').popover({
            container: 'body',
            html: true
        });
        ko.applyBindings(new Json2tsViewModel());
    });
})(json2ts || (json2ts = {}));
//# sourceMappingURL=Default.js.map