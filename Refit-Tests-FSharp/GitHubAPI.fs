namespace Refit.Tests.FSharp

open System
open System.Net.Http
open System.Threading.Tasks
open Refit

    type public User = {
        Login: string;
        Id: int;
        AvatarUrl: string;
        GravatarId: string;
        Url: string;
        HtmlUrl: string;
        FollowersUrl: string;
        FollowingUrl: string;
        GistsUrl: string;
        StarredUrl: string;
        SubscriptionsUrl: string;
        OrganizationsUrl: string;
        ReposUrl: string;
        EventsUrl: string;
        ReceivedEventsUrl: string;
        Type: string;
        Name: string
        Company: string;
        Blog: string;
        Location: string;
        Email: string;
        Hireable: bool;
        Bio: string;
        PublicRepos: int;
        Followers: int;
        Following: int;
        CreatedAt: string;
        UpdatedAt: string;
        PublicGists: int;
    }

    type public UserSearchResult = {
        TotalCount: int;
        IncompleteResults: bool;
        Items: User list;
    }

    [<Headers("User-Agent: Refit Integration Tests")>]
    type public IGitHubApi =

      [<Get("/users/{username}")>]
      abstract member GetUser: userName:string -> Task<User>

      [<Get("/users/{username}")>]
      abstract member GetUserObservable: userName:string -> IObservable<User>

      [<Get("/users/{userName}")>]
      abstract member GetUserCamelCase: userName:string -> IObservable<User>

      [<Get("/orgs/{orgname}/members")>]
      abstract member GetOrgMembers: orgName:string -> Task<List<User>>

      [<Get("/search/users")>]
      abstract member FindUsers: q:string -> Task<UserSearchResult>

      [<Get("/")>]
      abstract member GetIndex: unit -> Task<HttpResponseMessage>

      [<Get("/")>]
      abstract member GetIndexObservable: unit -> IObservable<string>

      [<Get("/give-me-some-404-action")>]
      abstract member NothingToSeeHere: unit -> Task