/*alert("Hello")*/
var JsFunctions = window.JsFunctions || {};
JsFunctions = {
    MermaidInitialize: function () {
        mermaid.initialize({
            startOnLoad: true,
            securityLevel: "loose",
            // Other options.
        });
    },

    MermaidRender: function () {
        mermaid.init();
    },

    MermaidRender2Async: function (Id, input) {
        var output = document.getElementById(Id);
        mermaid.mermaidAPI.renderAsync(Id + '_Graph', input, function (svgCode) {
            output.innerHTML = svgCode;
        });
    },

    generateMermaidSvg: function (diagramText) {
        var rtnVal;

        mermaid.mermaidAPI.render('theGraph', diagramText, function (svgCode) {
            //mermaid.mermaidAPI.render(Id + "_Graph", diagramText, function (svgCode) {
            rtnVal = svgCode;
        });

        return rtnVal;
    }
};
