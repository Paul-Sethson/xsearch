package scalasearch

import java.io.File

class SearchFile(val containers: List[String], val path: String, val file: String) {

  val CONTAINER_SEPARATOR = "::"

  def this(path: String, file: String) = {
    this(List.empty[String], path, file)
  }

  def toFile: File = {
    val p = new File(path)
    new File(p, file)
  }

  // get just the path inside the container(s)
  def getPath = {
    toFile.getPath
  }

  // get just the path inside the container(s)
  def getPathWithContainers = {
    toString
  }

  override def toString = {
    val sb = new StringBuilder
    if (containers.nonEmpty) {
      sb.append(containers.mkString(CONTAINER_SEPARATOR)).append(CONTAINER_SEPARATOR)
    }
    sb.append(path).append(File.separator).append(file)
    sb.toString()
  }
}
