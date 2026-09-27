export class regex_patterns {
    static readonly username = /^([a-zA-Z0-9._-]+\\[a-zA-Z0-9._-]+|[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}|[a-zA-Z0-9._-]+)$/;
    static readonly remote = /^(https?:\/\/)?[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*$|^$/;
}
