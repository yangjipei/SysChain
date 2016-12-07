(function() {
    var URL = window.UEDITOR_HOME_URL || getUEBasePath();

    window.UEDITOR_CONFIG = {
        UEDITOR_HOME_URL: URL,
        serverUrl: URL + "php/controller.php",
        toolbars: [
            ['fullscreen', 'source', '|', 'undo', 'redo', '|',
                'bold', 'italic', 'underline', 'fontborder', 'strikethrough', 'superscript', 'subscript', 'removeformat', 'formatmatch', 'autotypeset', 'blockquote', 'pasteplain', '|', 'forecolor', 'backcolor', 'insertorderedlist', 'insertunorderedlist', 'selectall', 'cleardoc', '|',
                'rowspacingtop', 'rowspacingbottom', 'lineheight', '|',
                'customstyle', 'paragraph', 'fontfamily', 'fontsize', '|',
                'directionalityltr', 'directionalityrtl', 'indent', '|',
                'justifyleft', 'justifycenter', 'justifyright', 'justifyjustify', '|', 'touppercase', 'tolowercase', '|',
                'link', 'unlink', 'anchor', '|', 'imagenone', 'imageleft', 'imageright', 'imagecenter', '|',
                'emotion', 'insertframe', 'pagebreak', 'template', 'background', 'simpleupload', 'insertimage', '|',
                'horizontal', 'date', 'time', 'spechars', 'wordimage', 'map', '|',
                'inserttable', 'edittable', 'edittd', 'deletetable', 'insertparagraphbeforetable', 'insertrow', 'deleterow', 'insertcol', 'deletecol', 'mergecells', 'mergeright', 'mergedown', 'splittocells', 'splittorows', 'splittocols', '|',
                'print', 'preview', 'searchreplace', 'help', 'drafts'
            ]
        ],
        zIndex: 0,
        charset: "utf-8",
        initialContent: '在使用编辑器过程中有任何疑问？或 所操作的与想预想不同，请在右侧提交您的问题？',
        initialStyle: 'p{line-height:1em;font-size:12px;}',
        enableAutoSave: true,
        saveInterval: 500,
        fullscreen: false,
        imagePopup: true,
        wordCount: true,
        maximumWords: 100000,
        autoHeightEnabled: false,
        topOffset: 0
    };

    function getUEBasePath(docUrl, confUrl) {
        return getBasePath(docUrl || self.document.URL || self.location.href, confUrl || getConfigFilePath());
    }

    function getConfigFilePath() {
        var configPath = document.getElementsByTagName('script');

        return configPath[configPath.length - 1].src;
    }

    function getBasePath(docUrl, confUrl) {
        var basePath = confUrl;

        if (/^(\/|\\\\)/.test(confUrl)) {
            basePath = /^.+?\w(\/|\\\\)/.exec(docUrl)[0] + confUrl.replace(/^(\/|\\\\)/, '');
        } else if (!/^[a-z]+:/i.test(confUrl)) {
            docUrl = docUrl.split("#")[0].split("?")[0].replace(/[^\\\/]+$/, '');
            basePath = docUrl + "" + confUrl;
        } //if

        return optimizationPath(basePath);
    }

    function optimizationPath(path) {
        var protocol = /^[a-z]+:\/\//.exec(path)[0],
            tmp = null,
            res = [];

        path = path.replace(protocol, "").split("?")[0].split("#")[0];
        path = path.replace(/\\/g, '/').split(/\//);
        path[path.length - 1] = "";

        while (path.length) {
            if ((tmp = path.shift()) === "..") {
                res.pop();
            } else if (tmp !== ".") {
                res.push(tmp);
            } //if
        } //while

        return protocol + res.join("/");
    }

    window.UE = {
        getUEBasePath: getUEBasePath
    };

})();