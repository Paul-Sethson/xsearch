package scalasearch

import java.io.File
import scala.util.matching.Regex

class SearchResult(val searchPattern: Regex, val file: File, val lineNum: Int,
    val line: String, linesBefore: List[String], linesAfter: List[String]) {

  def this(searchPattern: Regex, file: File, lineNum: Int, line: String) = {
    this(searchPattern, file, lineNum, line, List[String](), List[String]())
  }

  val sepLen = 80

  override def toString = {
    if (linesBefore.nonEmpty || linesAfter.nonEmpty)
      multilineToString
    else
      singleLineToString
  }

  def singleLineToString = {
    val matchString =
      if (lineNum == 0) " matches"
      else ": %d: %s".format(lineNum, line.trim)
    file.getPath + matchString
  }

  def lineNumPadding: Int = {
    val maxLineNum = lineNum + linesAfter.length
    "%d".format(maxLineNum).length
  }

  def multilineToString = {
    val sb = new StringBuilder
    sb.append("%s\n%s\n%s\n".format("=" * sepLen, file.getPath, "-" * sepLen))
    val lineFormat = " %1$" + lineNumPadding + "d | %2$s\n"
    var currentLineNum = lineNum
    if (linesBefore.length > 0) {
      currentLineNum -= linesBefore.length
      for (lineBefore <- linesBefore) {
        sb.append(" " + lineFormat.format(currentLineNum, lineBefore))
        currentLineNum += 1
      }
    }
    sb.append(">" + lineFormat.format(lineNum, line))
    if (linesAfter.length > 0) {
      currentLineNum += 1
      for (lineAfter <- linesAfter) {
        sb.append(" " + lineFormat.format(currentLineNum, lineAfter))
        currentLineNum += 1
      }
    } else sb.append('\n')
    sb.toString()
  }
}
