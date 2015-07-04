namespace Refit.Tests.FSharp

open System.Threading.Tasks
open Refit

type public IUseOverloadedMethods =

    [<Get("/")>]
    abstract member Get: unit -> Task

    [<Get("/{id}")>]
    abstract member Get: id:int -> Task
