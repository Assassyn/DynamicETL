module AraneaIT.Migration.Engine.Tests.RegexUnitTest

open Xunit
open System
open System.Linq
open System.Text.RegularExpressions

type test() =
    let csvREgex = "(?<=^|,)(\"(?:[^\"]|\"\")*\"|[^,]*)"

    [<Fact>]
    member this.MatchNonQueriedString() = 
        let line = "example item 1, example item 2"
        let results = Regex.Matches(line, csvREgex)
        Assert.Equal(2, results.Count)

    [<Fact>]
    member this.MatchQueriedStringOnly() = 
        let line = "\"example item 1\",\"example item 2\""
        let results = Regex.Matches(line, csvREgex)
        Assert.Equal(2, results.Count)

    [<Fact>]
    member this.MatchMixedQueriedStringOnly() = 
        let line = "\"example item 1\",example item 2"
        let results = Regex.Matches(line, csvREgex)
        Assert.Equal(2, results.Count)