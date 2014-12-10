/*
 * nodesearch.js
 *
 * file search utility written in node.js
 */

function SearchSettings() {
    var that = this;
    this.DEFAULT_OUT_DIRPATTERNS = [
        new RegExp("\\bCVS"),
        new RegExp("\\.git"),
        new RegExp("\\.svn")]
    this.DEFAULT_OUT_FILEPATTERNS = [new RegExp("^\\.DS_Store$")]
    this.startPath = "";
    this.inExtensions = [];
    this.outExtensions = [];
    this.inDirPatterns = [];
    this.outDirPatterns = this.DEFAULT_OUT_DIRPATTERNS;
    this.inFilePatterns = [];
    this.outFilePatterns = this.DEFAULT_OUT_FILEPATTERNS;
    this.inArchiveFilePatterns = [];
    this.outArchiveFilePatterns = [];
    this.inLinesAfterPatterns = [];
    this.outLinesAfterPatterns = [];
    this.inLinesBeforePatterns = [];
    this.outLinesBeforePatterns = [];
    this.linesAfterToPatterns = [];
    this.linesAfterUntilPatterns = [];
    this.searchPatterns = [];
    this.archivesOnly = false;
    this.debug = false;
    this.doTiming = false;
    this.excludeHidden = true;
    this.firstMatch = false;
    this.linesAfter = 0;
    this.linesBefore = 0;
    this.listDirs = false;
    this.listFiles = false;
    this.listLines = false;
    this.maxLineLength = 150;
    this.multilineSearch = false;
    this.printResults = false;
    this.printUsage = false;
    this.printVersion = false;
    this.recursive = true;
    this.searchArchives = false;
    this.uniqueLines = false;
    this.verbose = false;
    var addExtension = function (ext, arr) {
        arr.push(ext);
    };
    this.addInExtension = function (ext) {
        addExtension(ext, that.inExtensions);
    };
    this.addOutExtension = function (ext) {
        addExtension(ext, that.outExtensions);
    };
    var addPattern = function (pattern, arr) {
        arr.push(new RegExp(pattern));
    };
    this.addInDirPattern = function (pattern) {
        addPattern(pattern, that.inDirPatterns);
    };
    this.addOutDirPattern = function (pattern) {
        addPattern(pattern, that.outDirPatterns);
    };
    this.addInFilePattern = function (pattern) {
        addPattern(pattern, that.inFilePatterns);
    };
    this.addOutFilePattern = function (pattern) {
        addPattern(pattern, that.outFilePatterns);
    };
    this.addSearchPattern = function (pattern) {
        addPattern(pattern, that.searchPatterns);
    };
    this.addInArchiveFilePattern = function (pattern) {
        addPattern(pattern, that.inArchiveFilePatterns);
    };
    this.addOutArchiveFilePattern = function (pattern) {
        addPattern(pattern, that.outArchiveFilePatterns);
    };
    this.addInLinesAfterPattern = function (pattern) {
        addPattern(pattern, that.inLinesAfterPatterns);
    };
    this.addOutLinesAfterPattern = function (pattern) {
        addPattern(pattern, that.outLinesAfterPatterns);
    };
    this.addInLinesBeforePattern = function (pattern) {
        addPattern(pattern, that.inLinesBeforePatterns);
    };
    this.addOutLinesBeforePattern = function (pattern) {
        addPattern(pattern, that.outLinesBeforePatterns);
    };

    this.addLinesAfterToPattern = function (pattern) {
        addPattern(pattern, that.linesAfterToPatterns);
    };
    this.addLinesAfterUntilPattern = function (pattern) {
        addPattern(pattern, that.linesAfterUntilPatterns);
    };

    this.toString = function () {
        var s = 'SearchSettings(startPath="' + that.startPath + '"';
        if (that.inExtensions.length) {
            s = s + ', inExtensions=["' + that.inExtensions.join('","') + '"]';
        }
        if (that.outExtensions.length) {
            s = s + ', outExtensions=["' + that.outExtensions.join('","') + '"]';
        }
        if (that.inDirPatterns.length) {
            s = s + ', inDirPatterns=["' + that.inDirPatterns.join('","') + '"]';
        }
        if (that.outDirPatterns.length) {
            s = s + ', outDirPatterns=["' + that.outDirPatterns.join('","') + '"]';
        }
        if (that.inFilePatterns.length) {
            s = s + ', inFilePatterns=["' + that.inFilePatterns.join('","') + '"]';
        }
        if (that.outFilePatterns.length) {
            s = s + ', outFilePatterns=["' + that.outFilePatterns.join('","') + '"]';
        }
        if (that.inArchiveFilePatterns.length) {
            s = s + ', inArchiveFilePatterns=["' + that.inArchiveFilePatterns.join('","') + '"]';
        }
        if (that.outArchiveFilePatterns.length) {
            s = s + ', outArchiveFilePatterns=["' + that.outArchiveFilePatterns.join('","') + '"]';
        }
        if (that.inLinesAfterPatterns.length) {
            s = s + ', inLinesAfterPatterns=["' + that.inLinesAfterPatterns.join('","') + '"]';
        }
        if (that.outLinesAfterPatterns.length) {
            s = s + ', outLinesAfterPatterns=["' + that.outLinesAfterPatterns.join('","') + '"]';
        }
        if (that.inLinesBeforePatterns.length) {
            s = s + ', inLinesBeforePatterns=["' + that.inLinesBeforePatterns.join('","') + '"]';
        }
        if (that.outLinesBeforePatterns.length) {
            s = s + ', outLinesBeforePatterns=["' + that.outLinesBeforePatterns.join('","') + '"]';
        }
        if (that.linesAfterToPatterns.length) {
            s = s + ', linesAfterToPatterns=["' + that.linesAfterToPatterns.join('","') + '"]';
        }
        if (that.linesAfterUntilPatterns.length) {
            s = s + ', linesAfterUntilPatterns=["' + that.linesAfterUntilPatterns.join('","') + '"]';
        }
        if (that.searchPatterns.length) {
            s = s + ', searchPatterns=["' + that.searchPatterns.join('","') + '"]';
        }
        s = s + ', archivesOnly=' + that.archivesOnly;
        s = s + ', debug=' + that.debug;
        s = s + ', doTiming=' + that.doTiming;
        s = s + ', excludeHidden=' + that.excludeHidden;
        s = s + ', firstMatch=' + that.firstMatch;
        s = s + ', linesAfter=' + that.linesAfter;
        s = s + ', linesBefore=' + that.linesBefore;
        s = s + ', listDirs=' + that.listDirs;
        s = s + ', listFiles=' + that.listFiles;
        s = s + ', listLines=' + that.listLines;
        s = s + ', maxLineLength=' + that.maxLineLength;
        s = s + ', multilineSearch=' + that.multilineSearch;
        s = s + ', printResults=' + that.printResults;
        s = s + ', printVersion=' + that.printVersion;
        s = s + ', recursive=' + that.recursive;
        s = s + ', searchArchives=' + that.searchArchives;
        s = s + ', uniqueLines=' + that.uniqueLines;
        s = s + ', verbose=' + that.verbose;
        s = s + ')';
        return s;
    };
}

exports.SearchSettings = SearchSettings;