import React, { Component } from "react";
import { Link } from "react-router-dom";
import "./NavMenu.css";

export class NavMenu extends Component {
  displayName = NavMenu.name;

  render() {
    return (
      <ul>
        <li>
          <Link to={"/"}>Home</Link>
        </li>
        <li>
          <Link to={"/counter"}>Counter</Link>
        </li>
        <li>
          <Link to={"/fetchdata"}>Fetch Data</Link>
        </li>
        <li>
          <Link to={"/login"}>Login</Link>
        </li>
      </ul>
    );
  }
}
