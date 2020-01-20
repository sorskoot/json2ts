/// <reference path="typings/knockout/knockout.d.ts" />
/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="timmytools.ts" />

module json2ts {

    class Json2tsViewModel {
        code = ko.observable<string>();
        modulename = ko.observable<string>("someModule");
        rootObject = ko.observable<string>("root");
        error = ko.observable<string>();
        inputErrorMessage = ko.observable<string>();
        resultcode: KnockoutObservable<string> = ko.observable<string>();

        constructor() {
        }
        

        getTS = () => {
            if (!this.code()) {
                this.inputErrorMessage("please enter JSON code or an URL to service that returns JSON");
                return;
            }
            this.callServer(this.code());
        };

        private callServer(code: string): void {
            if (!TimmyTools.IsValidJson(code) && code.substr(0, 4) != 'http') {
                this.inputErrorMessage("There seems to be an error in the JSON");
                return;
            }
            this.inputErrorMessage(null);
            code = code.replace(/\</g, "");
            $.post("/Home/GetTypeScriptDefinition", { code: code, ns: this.modulename(), root: this.rootObject() }, (result) => {
                if (!result.error) {
                    this.error(null);
                    this.resultcode(result);
                } else {
                    this.error(result.error);
                }
            });
        }
    }
    interface errorResponse {
        status: number;
        statusText: string;
    }

    $(document).ready(() => {
        $('#help').popover({
            container: 'body',
            html: true
        });
        ko.applyBindings(new Json2tsViewModel());
    });
}