package xsearch

import "regexp"
import "testing"

func TestAddSearchResult(t *testing.T) {
	searchResults := NewSearchResults()

	path := "~/src/git/xsearch/go/src/github.com/clarkcb/gosearch"
	name := "searchresults_test.go"
	searchItem := NewSearchItem(&path, &name)

	line := "func TestAddSearchResult(t *testing.T) {"

	searchResult := &SearchResult{
		regexp.MustCompile("^func\\s+TestAddSearchResult"),
		searchItem,
		6,
		1,
		25,
		&line,
		[]*string{},
		[]*string{},
	}
	searchResults.AddSearchResult(searchResult)

	if searchResults.IsEmpty() {
		t.Errorf("searchResults.IsEmpty()=%t, expected=false",
			searchResults.IsEmpty())
	}

	if len(searchResults.SearchResults) != 1 {
		t.Errorf("len(searchResults.SearchResults)=%d, expected=1",
			len(searchResults.SearchResults))
	}

	expectedDir := "~/src/git/xsearch/go/src/github.com/clarkcb/gosearch"
	c, b := searchResults.DirCounts[expectedDir]
	if !b {
		t.Errorf("searchResults.DirCounts does not contain expected dir key: %s",
			expectedDir)
	}
	if c != 1 {
		t.Errorf("searchResults.DirCounts[\"%s\"]=%d, expected=1",
			expectedDir, searchResults.DirCounts[expectedDir])
	}

	expectedFile := "~/src/git/xsearch/go/src/github.com/clarkcb/gosearch/searchresults_test.go"
	c, b = searchResults.FileCounts[expectedFile]
	if !b {
		t.Errorf("searchResults.FileCounts does not contain expected file key: %s",
			expectedFile)
	}
	if c != 1 {
		t.Errorf("searchResults.FileCounts[\"%s\"]=%d, expected=1",
			expectedFile, searchResults.FileCounts[expectedFile])
	}

}
