import { IdentityError } from "./IdentityError";

export class TokenResult {
  constructor(public token: string, public errors: IdentityError[]) {}

  static success = (token: string) => new TokenResult(token, []);
  static failed = (errors: IdentityError[]) => new TokenResult("", errors);
}
