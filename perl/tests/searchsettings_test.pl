#!/usr/bin/perl -w
#
# searchsettings_test.pl
#
#
use strict;
use warnings;

BEGIN {
  use lib "$ENV{HOME}/src/git/xsearch/perl";
}

use Test::Simple tests => 25;

use plsearch::SearchSettings;

sub test_default_settings {
    my $settings = new plsearch::SearchSettings();
    ok(!$settings->{archivesonly}, "archivesonly is false by default");
    ok(!$settings->{debug}, "debug is false by default");
    ok(!$settings->{dotiming}, "dotiming is false by default");
    ok($settings->{excludehidden}, "excludehidden is true by default");
    ok(!$settings->{firstmatch}, "firstmatch is false by default");
    ok($settings->{linesafter} == 0, "linesafter == 0 by default");
    ok($settings->{linesbefore} == 0, "linesbefore == 0 by default");
    ok(!$settings->{listdirs}, "listdirs is false by default");
    ok(!$settings->{listfiles}, "listfiles is false by default");
    ok(!$settings->{listlines}, "listlines is false by default");
    ok($settings->{maxlinelength} == 150, "maxlinelength == 150 by default");
    ok(!$settings->{multilinesearch}, "multilinesearch is false by default");
    ok($settings->{printresults}, "printresults is true by default");
    ok(!$settings->{printusage}, "printusage is false by default");
    ok(!$settings->{printversion}, "printversion is false by default");
    ok($settings->{recursive}, "recursive is true by default");
    ok(!$settings->{searcharchives}, "searcharchives is false by default");
    ok($settings->{startpath} eq '', "startpath is empty by default");
    ok(!$settings->{uniquelines}, "uniquelines is false by default");
    ok(!$settings->{verbose}, "verbose is false by default");
}

sub test_add_single_extension {
    my $settings = new plsearch::SearchSettings();
    $settings->add_exts('pl', $settings->{in_extensions});
    ok(scalar @{$settings->{in_extensions}} == 1, "in_extensions has one extension");
    ok($settings->{in_extensions}->[0] eq 'pl', "in_extensions contains pl extension");
}

sub test_add_comma_delimited_extensions {
    my $settings = new plsearch::SearchSettings();
    $settings->add_exts('pl,py', $settings->{in_extensions});
    ok(scalar @{$settings->{in_extensions}} == 2, "in_extensions has two extensions");
    ok($settings->{in_extensions}->[0] eq 'pl', "in_extensions contains pl extension");
    ok($settings->{in_extensions}->[1] eq 'py', "in_extensions contains py extension");
}

sub main {
    test_default_settings();
    test_add_single_extension();
    test_add_comma_delimited_extensions();
}

main();
