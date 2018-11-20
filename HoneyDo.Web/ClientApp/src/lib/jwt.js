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
  return {
    id: decoded.jti,
    name: decoded.sub,
    token: token,
    isValid: true
  };
};
