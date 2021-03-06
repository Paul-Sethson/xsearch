/*******************************************************************************
FileUtil

Utility class to determine file types, etc.

@author Cary Clark &lt;clarkcb@gmail.com&gt;
@version $Rev$
@copyright Cary Clark 2012
*******************************************************************************/

package javasearch;

import org.apache.commons.io.FileUtils;
import org.apache.commons.io.LineIterator;

import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashSet;
import java.util.List;
import java.util.NoSuchElementException;
import java.util.Set;
import java.util.Scanner;

public class FileUtil {

    private static Set<String> dotDirs = new HashSet<String>(Arrays.asList(".", ".."));

    public static String getExtension(final File f) {
        String ext = "";
        String fileName = f.getName();
        int lastIndex = fileName.lastIndexOf(".");
        if (lastIndex > 0 && lastIndex < fileName.length() - 1)
            ext = fileName.substring(lastIndex + 1);
        return ext;
    }

    public static boolean isDotDir(final String f) {
        return dotDirs.contains(f);
    }

    public static boolean isHidden(final File f) {
        return isHidden(f.getName());
    }

    public static boolean isHidden(final String f) {
        return f.length() > 1 && f.charAt(0) == '.' && !isDotDir(f);
    }

    public static List<String> splitPath(final String path) {
        String[] elems = path.split(File.separator);
        List<String> nonDotDirElems = new ArrayList<String>();
        for (String elem : elems) {
            if (!isDotDir(elem) && !elem.equals("")) nonDotDirElems.add(elem);
        }
        return nonDotDirElems;
    }

    public static String getFileContents(final File f) throws IOException {
        try {
            Scanner scanner = new Scanner(f, "ISO8859-1").useDelimiter("\\Z");
            String content;
            try {
                content = scanner.next();
            } catch (NoSuchElementException e) {
                throw e;
            } catch (IllegalStateException e) {
                throw e;
            } finally {
                scanner.close();
            }
            return content;
        } catch (IOException e) {
            throw e;
        }

    }

    // NOTE: user takes responsibility for closing the LineIterator once done
    public static LineIterator getFileLineIterator(final File f) throws IOException {
        LineIterator it = null;
        try {
            it = FileUtils.lineIterator(f, "ISO8859-1");
            return it;
        } catch (IOException e) {
            throw e;
        }
    }
}
