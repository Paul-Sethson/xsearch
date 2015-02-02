# -*- coding: utf-8 -*-
################################################################################
#
# searcher_test.py
#
# Searcher testing
#
################################################################################
import os
import sys
import unittest

sys.path.insert(0, '%s/src/git/xsearch/python' % os.environ['HOME'])

from pysearch.filetypes import FileType
from pysearch.searcher import Searcher
from pysearch.searchfile import SearchFile
from pysearch.searchsettings import SearchSettings

class SearcherTest(unittest.TestCase):

    def get_settings(self):
        settings = SearchSettings()
        settings.startpath = '.'
        settings.add_pattern('Searcher', 'searchpatterns')
        return settings

################################################################################
# is_search_dir tests
################################################################################

    def test_is_search_dir_no_patterns(self):
        settings = self.get_settings()
        searcher = Searcher(settings)
        dir = 'plsearch'
        self.assertTrue(searcher.is_search_dir(dir))

    def test_is_search_dir_matches_in_pattern(self):
        settings = self.get_settings()
        settings.add_pattern('plsearch', 'in_dirpatterns')
        searcher = Searcher(settings)
        dir = 'plsearch'
        self.assertTrue(searcher.is_search_dir(dir))

    def test_is_search_dir_no_match_in_pattern(self):
        settings = self.get_settings()
        settings.add_pattern('plsearch', 'in_dirpatterns')
        searcher = Searcher(settings)
        dir = 'pysearch'
        self.assertFalse(searcher.is_search_dir(dir))

    def test_is_search_dir_matches_out_pattern(self):
        settings = self.get_settings()
        settings.add_pattern('pysearch', 'out_dirpatterns')
        searcher = Searcher(settings)
        dir = 'pysearch'
        self.assertFalse(searcher.is_search_dir(dir))

    def test_is_search_dir_no_match_out_pattern(self):
        settings = self.get_settings()
        settings.add_pattern('pysearch', 'out_dirpatterns')
        searcher = Searcher(settings)
        dir = 'plsearch'
        self.assertTrue(searcher.is_search_dir(dir))

    def test_is_search_dir_single_dot(self):
        settings = self.get_settings()
        searcher = Searcher(settings)
        dir = '.'
        self.assertTrue(searcher.is_search_dir(dir))

    def test_is_search_dir_double_dot(self):
        settings = self.get_settings()
        searcher = Searcher(settings)
        dir = '..'
        self.assertTrue(searcher.is_search_dir(dir))

    def test_is_search_dir_hidden_dir(self):
        settings = self.get_settings()
        searcher = Searcher(settings)
        dir = '.git'
        self.assertFalse(searcher.is_search_dir(dir))

    def test_is_search_dir_hidden_dir_include_hidden(self):
        settings = self.get_settings()
        settings.excludehidden = False
        searcher = Searcher(settings)
        dir = '.git'
        self.assertTrue(searcher.is_search_dir(dir))

################################################################################
# is_search_file tests
################################################################################

    def test_is_search_file_matches_by_default(self):
        settings = self.get_settings()
        searcher = Searcher(settings)
        f = 'FileUtil.pm'
        self.assertTrue(searcher.is_search_file(f))

    def test_is_search_file_matches_in_extension(self):
        settings = self.get_settings()
        settings.add_comma_delimited_exts('pm', 'in_extensions')
        searcher = Searcher(settings)
        f = 'FileUtil.pm'
        self.assertTrue(searcher.is_search_file(f))

    def test_is_search_file_no_match_in_extension(self):
        settings = self.get_settings()
        settings.add_comma_delimited_exts('pl', 'in_extensions')
        searcher = Searcher(settings)
        f = 'FileUtil.pm'
        self.assertFalse(searcher.is_search_file(f))

    def test_is_search_file_matches_out_extension(self):
        settings = self.get_settings()
        settings.add_comma_delimited_exts('pm', 'out_extensions')
        searcher = Searcher(settings)
        f = 'FileUtil.pm'
        self.assertFalse(searcher.is_search_file(f))

    def test_is_search_file_no_match_out_extension(self):
        settings = self.get_settings()
        settings.add_comma_delimited_exts('py', 'out_extensions')
        searcher = Searcher(settings)
        f = 'FileUtil.pm'
        self.assertTrue(searcher.is_search_file(f))

    def test_is_search_file_matches_in_pattern(self):
        settings = self.get_settings()
        settings.add_pattern('Search', 'in_filepatterns')
        searcher = Searcher(settings)
        f = 'Searcher.pm'
        self.assertTrue(searcher.is_search_file(f))

    def test_is_search_file_no_match_in_pattern(self):
        settings = self.get_settings()
        settings.add_pattern('Search', 'in_filepatterns')
        searcher = Searcher(settings)
        f = 'FileUtil.pm'
        self.assertFalse(searcher.is_search_file(f))

    def test_is_search_file_matches_out_pattern(self):
        settings = self.get_settings()
        settings.add_pattern('Search', 'out_filepatterns')
        searcher = Searcher(settings)
        f = 'Searcher.pm'
        self.assertFalse(searcher.is_search_file(f))

    def test_is_search_file_no_match_out_pattern(self):
        settings = self.get_settings()
        settings.add_pattern('Search', 'out_filepatterns')
        searcher = Searcher(settings)
        f = 'FileUtil.pm'
        self.assertTrue(searcher.is_search_file(f))

################################################################################
# is__archive_search_file tests
################################################################################

    def test_is_archive_search_file_matches_by_default(self):
        settings = self.get_settings()
        searcher = Searcher(settings)
        f = 'archive.zip'
        self.assertTrue(searcher.is_archive_search_file(f))

    def test_is_archive_search_file_matches_in_extension(self):
        settings = self.get_settings()
        settings.add_comma_delimited_exts('zip', 'in_archiveextensions')
        searcher = Searcher(settings)
        f = 'archive.zip'
        self.assertTrue(searcher.is_archive_search_file(f))

    def test_is_archive_search_file_no_match_in_extension(self):
        settings = self.get_settings()
        settings.add_comma_delimited_exts('gz', 'in_archiveextensions')
        searcher = Searcher(settings)
        f = 'archive.zip'
        self.assertFalse(searcher.is_archive_search_file(f))

    def test_is_archive_search_file_matches_out_extension(self):
        settings = self.get_settings()
        settings.add_comma_delimited_exts('zip', 'out_archiveextensions')
        searcher = Searcher(settings)
        f = 'archive.zip'
        self.assertFalse(searcher.is_archive_search_file(f))

    def test_is_archive_search_file_no_match_out_extension(self):
        settings = self.get_settings()
        settings.add_comma_delimited_exts('gz', 'out_archiveextensions')
        searcher = Searcher(settings)
        f = 'archive.zip'
        self.assertTrue(searcher.is_archive_search_file(f))

    def test_is_archive_search_file_matches_in_pattern(self):
        settings = self.get_settings()
        settings.add_pattern('arch', 'in_archivefilepatterns')
        searcher = Searcher(settings)
        f = 'archive.zip'
        self.assertTrue(searcher.is_archive_search_file(f))

    def test_is_archive_search_file_no_match_in_pattern(self):
        settings = self.get_settings()
        settings.add_pattern('archives', 'in_archivefilepatterns')
        searcher = Searcher(settings)
        f = 'archive.zip'
        self.assertFalse(searcher.is_archive_search_file(f))

    def test_is_archive_search_file_matches_out_pattern(self):
        settings = self.get_settings()
        settings.add_pattern('arch', 'out_archivefilepatterns')
        searcher = Searcher(settings)
        f = 'archive.zip'
        self.assertFalse(searcher.is_archive_search_file(f))

    def test_is_archive_search_file_no_match_out_pattern(self):
        settings = self.get_settings()
        settings.add_pattern('archives', 'out_archivefilepatterns')
        searcher = Searcher(settings)
        f = 'archive.zip'
        self.assertTrue(searcher.is_archive_search_file(f))

################################################################################
# filter_file tests
################################################################################

    def test_filter_file_matches_by_default(self):
        settings = self.get_settings()
        searcher = Searcher(settings)
        f = SearchFile(path='', filename='FileUtil.pm', filetype=FileType.Text)
        self.assertTrue(searcher.filter_file(f))

    def test_filter_file_is_search_file(self):
        settings = self.get_settings()
        settings.add_comma_delimited_exts('pm', 'in_extensions')
        searcher = Searcher(settings)
        f = SearchFile(path='', filename='FileUtil.pm', filetype=FileType.Text)
        self.assertTrue(searcher.filter_file(f))

    def test_filter_file_not_is_search_file(self):
        settings = self.get_settings()
        settings.add_comma_delimited_exts('pl', 'in_extensions')
        searcher = Searcher(settings)
        f = SearchFile(path='', filename='FileUtil.pm', filetype=FileType.Text)
        self.assertFalse(searcher.filter_file(f))

    def test_filter_file_is_hidden_file(self):
        settings = self.get_settings()
        searcher = Searcher(settings)
        f = SearchFile(path='', filename='.gitignore', filetype=FileType.Unknown)
        self.assertFalse(searcher.filter_file(f))

    def test_filter_file_hidden_includehidden(self):
        settings = self.get_settings()
        settings.excludehidden = False
        searcher = Searcher(settings)
        f = SearchFile(path='', filename='.gitignore', filetype=FileType.Unknown)
        self.assertTrue(searcher.filter_file(f))

    def test_filter_file_archive_no_searcharchives(self):
        settings = self.get_settings()
        searcher = Searcher(settings)
        f = SearchFile(path='', filename='archive.zip', filetype=FileType.Archive)
        self.assertFalse(searcher.filter_file(f))

    def test_filter_file_archive_searcharchives(self):
        settings = self.get_settings()
        settings.searcharchives = 1
        searcher = Searcher(settings)
        f = SearchFile(path='', filename='archive.zip', filetype=FileType.Archive)
        self.assertTrue(searcher.filter_file(f))

    def test_filter_file_archive_archivesonly(self):
        settings = self.get_settings()
        settings.archivesonly = True
        settings.searcharchives = True
        searcher = Searcher(settings)
        f = SearchFile(path='', filename='archive.zip', filetype=FileType.Archive)
        self.assertTrue(searcher.filter_file(f))

    def test_filter_file_nonarchive_archivesonly(self):
        settings = self.get_settings()
        settings.archivesonly = True
        settings.searcharchives = True
        searcher = Searcher(settings)
        f = SearchFile(path='', filename='FileUtil.pm', filetype=FileType.Text)
        self.assertFalse(searcher.filter_file(f))

if __name__ == '__main__':
    unittest.main()
