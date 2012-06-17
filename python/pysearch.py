#!/usr/bin/env python
################################################################################
#
# pysearch.py
#
# A file search utility implemented in python (2.x)
#
################################################################################
import sys

from searcher import Searcher
from searchoptions import SearchOptions

DEBUG = False

def main():
    if DEBUG:
        print 'sys.argv(%d): %s' % (len(sys.argv),str(sys.argv))

    searchoptions = SearchOptions()

    if len(sys.argv) < 4:
        print searchoptions.get_usage_string()

    settings = None
    try:
        settings = searchoptions.search_settings_from_args(sys.argv[1:])
    except Exception, e:
        print 'Exception: %s' % e
        print searchoptions.get_usage_string()
        sys.exit(1)

    if settings.printusage:
        print searchoptions.get_usage_string()
        sys.exit(1)

    if settings.printversion:
        print 'Version: 0.1'
        sys.exit(1)

    if DEBUG:
        settings.debug = True

    if settings.debug:
        print 'settings: %s' % str(settings)

    try:
        searcher = Searcher(settings)
        searcher.search()
    except KeyboardInterrupt:
        print
        sys.exit(0)


if __name__ == '__main__':
    main()
