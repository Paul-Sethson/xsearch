package xsearch

func GetFileTypes() *FileTypes {
	return &FileTypes{
		map[string]set{
			"archive": makeSet([]string{"Z", "bz2", "cpio", "ear", "gz", "hqx", "jar", "pax", "rar", "sit", "sitx", "tar", "tgz", "war", "zip", "zipx"}),
			"binary": makeSet([]string{"a", "ai", "beam", "bin", "chm", "class", "com", "dat", "dbmdl", "dcr", "dir", "dll", "dms", "doc", "dot", "dxr", "dylib", "exe", "hi", "hlp", "indd", "lnk", "mdb", "mo", "nib", "o", "obj", "odt", "pdb", "ppt", "psd", "pyc", "pyo", "qxd", "so", "swf", "sys", "vsd", "xls", "xlt"}),
			"code": makeSet([]string{"applejs", "as", "bash", "bat", "boo", "bsh", "c", "c++", "cc", "cgi", "clj", "coffee", "cpp", "cs", "csh", "cxx", "el", "erl", "es", "fs", "fx", "go", "groovy", "h", "h++", "hh", "hpp", "hs", "java", "js", "js2", "jsf", "json", "jsp", "jspf", "m", "pas", "php", "php3", "php4", "php5", "pl", "pm", "pxd", "pxi", "py", "pyw", "pyx", "rb", "rs", "sbt", "sc", "scala", "sh", "tcl", "vb"}),
			"nosearch": makeSet([]string{"aif", "aifc", "aiff", "au", "avi", "bmp", "cab", "db", "dmg", "eps", "gif", "icns", "ico", "idlk", "ief", "iso", "jpe", "jpeg", "jpg", "m3u", "m4a", "m4p", "mov", "movie", "mp3", "mp4", "mpe", "mpeg", "mpg", "mxu", "ogg", "pdf", "pict", "png", "ps", "qt", "ra", "ram", "rm", "rpm", "snd", "suo", "tif", "tiff", "wav"}),
			"text": makeSet([]string{"1", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "2", "20", "3", "323", "4", "5", "6", "7", "8", "9", "am", "app", "asc", "ascx", "asm", "asp", "aspx", "bib", "brf", "cabal", "cfg", "cls", "cmd", "cnt", "conf", "css", "csv", "ctl", "d", "dbml", "dbschema", "ddl", "dep", "dfm", "diff", "disco", "dlg", "dof", "dpr", "drl", "dsp", "dsw", "dtd", "elt", "ent", "env", "etx", "exp", "feature", "fls", "gcd", "hql", "hs", "htc", "htm", "html", "hxx", "ics", "icz", "iml", "in", "inc", "ini", "ipr", "iws", "jad", "jam", "jql", "layout", "lhs", "log", "ltx", "mak", "mako", "manifest", "map", "markdown", "master", "md", "mf", "mht", "mml", "moc", "mod", "mxml", "p", "patch", "plist", "pm", "po", "pot", "properties", "pt", "rc", "rc2", "rdf", "rex", "rtf", "rtx", "scc", "sct", "sfv", "sgm", "sgml", "sht", "shtm", "shtml", "sln", "smi", "smil", "spec", "sqc", "sql", "st", "str", "strings", "sty", "suml", "sxw", "t", "tex", "text", "tk", "tld", "tm", "tmx", "tsv", "txt", "ui", "uls", "uml", "url", "user", "vbs", "vcf", "vcs", "vm", "vrml", "vssscc", "vxml", "wbxml", "webinfo", "wml", "wmls", "wrl", "wsc", "wsd", "wsdd", "xlf", "yaml", "yml"}),
			"unknown": makeSet([]string{"adm", "aps", "cli", "clw", "def", "df2", "ncb", "nt", "nt2", "orig", "pc", "plg", "roff", "sun", "texinfo", "tr", "xwd"}),
			"xml": makeSet([]string{"atom", "atomcat", "atomsrv", "bdsproj", "config", "csproj", "davmount", "dbproj", "docx", "dotx", "fsproj", "fxml", "jhm", "jnlp", "kml", "pom", "potx", "ppsx", "pptx", "qrc", "rdf", "resx", "rng", "rss", "settings", "sldx", "stc", "std", "sti", "stw", "svg", "svgz", "sxc", "sxd", "sxg", "sxi", "sxm", "sxw", "tld", "vbproj", "vcproj", "vdproj", "wadl", "wsdd", "wsdl", "x3d", "xaml", "xht", "xhtml", "xjb", "xlsx", "xltx", "xml", "xsd", "xsl", "xslt", "xspf", "xul"}),
		},
	}
}
