import jwtDecodeToken from "jwt-decode";

export const setToken = token => {
  localStorage.setItem("token", token);
};

export const getToken = () => {
  return localStorage.getItem("token");
};

export const decodeToken = token => {
  return jwtDecodeToken(token);
};

export const getUserData = () => {
  const token = getToken();
  if (!token || token.length === 0) {
    return { isValid: false };
  }
  const decoded = decodeToken(token);
  const expires = new Date(decoded.exp * 1000);
  const user = {
    id: decoded.jti,
    name: decoded.sub,
    token: token,
    expires,
    isValid: true
  };
  user.isExpired = function() {
    return this.expires <= new Date();
  }.bind(user);
  return user;
};
