namespace Refit.Tests.FSharp

open System
open System.Net
open System.Net.Http
open System.Reflection
open System.Threading
open System.Threading.Tasks
open Refit

[<Headers("User-Agent: RefitTestClient", "Api-Version: 1")>]
type public IRestMethodInfoTests =

    [<Get("@)!@_!($_!@($\\\\|||::::")>]
    abstract member GarbagePath: unit -> Task<string>

    [<Get("/foo/bar/{id}")>]
    abstract member FetchSomeStuffMissingParameters: unit -> Task<string> 

    [<Get("/foo/bar/{id}")>]
    abstract member FetchSomeStuff: id:int -> Task<string>

    [<Get("/foo/bar/{id}?baz=bamf")>]
    abstract member FetchSomeStuffWithHardcodedQueryParam: id:int -> Task<string>

    [<Get("/foo/bar/{id}?baz=bamf")>]
    abstract member FetchSomeStuffWithQueryParam: id:int * search:string -> Task<string>

    [<Get("/foo/bar/{id}")>]
    abstract member FetchSomeStuffWithAlias: [<AliasAs("id")>] anId:int -> Task<string>

    [<Get("/foo/bar/{width}x{height}")>]
    abstract member FetchAnImage: width:int * height:int -> Task<string>

    [<Get("/foo/bar/{id}")>]
    abstract member FetchSomeStuffWithBody: [<AliasAs("id")>] anId:int * [<Body>] theData:Map<int, string> -> IObservable<string>

    [<Post("/foo/bar/{id}")>]
    abstract member PostSomeUrlEncodedStuff: [<AliasAs("id")>] anId:int * [<Body(BodySerializationMethod.UrlEncoded)>] theData:Map<string, string> -> IObservable<string>

    [<Get("/foo/bar/{id}")>]
    [<Headers("Api-Version: 2 ")>]
    abstract member FetchSomeStuffWithHardcodedHeaders: id:int -> Task<string>

    [<Get("/foo/bar/{id}")>]
    abstract member FetchSomeStuffWithDynamicHeader: id:int * [<Header("Authorization")>] authorization:string -> Task<string>
    
    [<Post("/foo/{id}")>]
    abstract member OhYeahValueTypes: id:int * [<Body>] whatever:int -> Task<bool>

    [<Post("/foo/{id}")>]
    abstract member VoidPost: id:int -> Task

    [<Post("/foo/{id}")>]
    abstract member AsyncOnlyBuddy: id:int -> string

    [<Patch("/foo/{id}")>]
    abstract member PatchSomething: id:int * [<Body>] someAttribute:string -> IObservable<string>






type public SomeRequestData() =

    [<AliasAs("rpn")>]
    member val ReadablePropertyName: string option = None with get, set


[<Headers("User-Agent: RefitTestClient", "Api-Version: 1")>]
type public IDummyHttpApi =

    [<Get("/foo/bar/{id}")>]
    abstract member FetchSomeStuff: id:int -> Task<string>

    [<Get("/foo/bar/{id}?baz=bamf")>]
    abstract member FetchSomeStuffWithHardcodedQueryParameter: id:int -> Task<string>

    [<Get("/foo/bar/{id}?baz=bamf")>]
    abstract member FetchSomeStuffWithHardcodedAndOtherQueryParameters: id:int * [<AliasAs("search_for")>] searchQuery:string -> Task<string>

    [<Get("/{id}/{width}x{height}/foo")>]
    abstract member FetchSomethingWithMultipleParametersPerSegment: id:int * width:int * height:int -> Task<string>

    [<Get("/foo/bar/{id}")>]
    [<Headers("Api-Version: 2")>]
    abstract member FetchSomeStuffWithHardcodedHeader: id:int -> Task<string>

    [<Get("/foo/bar/{id}")>]
    [<Headers("Api-Version")>]
    abstract member FetchSomeStuffWithNullHardcodedHeader: id:int -> Task<string>

    [<Get("/foo/bar/{id}")>]
    [<Headers("Api-Version: ")>]
    abstract member FetchSomeStuffWithEmptyHardcodedHeader: id:int -> Task<string>

    [<Post("/foo/bar/{id}")>]
    [<Headers("Content-Type: literally/anything")>]
    abstract member PostSomeStuffWithHardCodedContentTypeHeader: id:int * [<Body>] content:string -> Task<string>

    [<Get("/foo/bar/{id}")>]
    [<Headers("Authorization: SRSLY aHR0cDovL2kuaW1ndXIuY29tL0NGRzJaLmdpZg==")>]
    abstract member FetchSomeStuffWithDynamicHeader: id:int * [<Header("Authorization")>] authorization:string -> Task<string>

    [<Get("/foo/bar/{id}")>]
    abstract member FetchSomeStuffWithCustomHeader: id:int * [<Header("X-Emoji")>] custom:string -> Task<string>

    [<Post("/foo/bar/{id}")>]
    abstract member PostSomeStuffWithCustomHeader: id:int * [<Body>] body:obj * [<Header("X-Emoji")>] emoji:string -> Task<string>

    [<Get("/string")>]
    abstract member FetchSomeStuffWithoutFullPath: unit -> Task<string>

    [<Get("/void")>]
    abstract member FetchSomeStuffWithVoid: unit -> Task

    [<Post("/foo/bar/{id}")>]
    abstract member PostSomeUrlEncodedStuff: id:int * [<Body(BodySerializationMethod.UrlEncoded)>] content:obj -> Task<string>

    [<Post("/foo/bar/{id}")>]
    abstract member PostSomeAliasedUrlEncodedStuff: id:int * [<Body(BodySerializationMethod.UrlEncoded)>] content:SomeRequestData -> Task<string>

    abstract member SomeOtherMethod: unit -> string

    [<Put("/foo/bar/{id}")>]
    abstract member PutSomeContentWithAuthorization: id:int * [<Body>] content:obj * [<Header("Authorization")>] authorization:string -> Task

    [<Put("/foo/bar/{id}")>]
    abstract member PutSomeStuffWithDynamicContentType: id:int * [<Body>] content:string * [<Header("Content-Type")>] contentType:string -> Task<string>

    [<Post("/foo/bar/{id}")>]
    abstract member PostAValueType: id:int * [<Body>] content:Guid option -> Task<bool>

    [<Patch("/foo/bar/{id}")>]
    abstract member PatchSomething: id:int * [<Body>] someAttribute:string -> IObservable<string>


type public TestHttpMessageHandler(?content: string) =
    inherit HttpMessageHandler()

    member val RequestMessage: HttpRequestMessage option = None with get, set
    member val MessagesSent: int = 0 with get, set
    member x.Content = defaultArg content "test"

    override x.SendAsync (request: HttpRequestMessage, cancellationToken: CancellationToken) =
        let result = new HttpResponseMessage(HttpStatusCode.OK)
        x.RequestMessage <- Some request
        x.MessagesSent <- x.MessagesSent + 1
        result.RequestMessage <- request
        result.Content <- new StringContent(x.Content)
        Task.FromResult result

type public TestUrlParameterFormatter(constantOutput: string) =
    interface IUrlParameterFormatter with
        
        member x.Format(value: obj, parameterInfo: ParameterInfo) =
            x.constantParameterOutput

    member x.constantParameterOutput = constantOutput
