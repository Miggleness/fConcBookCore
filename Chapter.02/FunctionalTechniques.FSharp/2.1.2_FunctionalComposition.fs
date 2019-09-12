module _2_1_2_FunctionalComposition

open System
open Xunit
open Shouldly



[<Trait("Category", "Chapter2")>]
[<Fact>]
let ``FunctionalCompositionTest`` () =

    // data
    let list = [0..2]
    let expectedList = [12; 15; 18]

    // add function
    let add4 x = x + 4

    // multiple function
    let multiplyBy3 x = x * 3
    
    // compose
    let actualList = list |> List.map(add4 >> multiplyBy3)

    // assert
    actualList.ShouldBe expectedList