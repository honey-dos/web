import React, { Component, ReactNode } from "react";
import PropTypes from "prop-types";
import AppBar from "../Navigation/AppBar";
import { createStyles, Theme, withStyles } from "@material-ui/core/styles";

const styles = ({ palette }: Theme) =>
  createStyles({
    root: {
      maxWidth: 800,
      marginLeft: "auto",
      marginRight: "auto",
      padding: 8
    }
  });

interface Props {
  children: Element | ReactNode;
  classes: { [key: string]: any };
}

interface LayoutState {
  isDrawerOpen: boolean;
}

const initialState: LayoutState = {
  isDrawerOpen: false
};

class Layout extends Component<Props, LayoutState> {
  static propTypes = {
    children: PropTypes.object,
    classes: PropTypes.object.isRequired
  };

  constructor(props: Props) {
    super(props);
    this.state = initialState;
  }

  toggleDrawer = () => {
    this.setState({ isDrawerOpen: !this.state.isDrawerOpen });
  };

  render() {
    const { children, classes } = this.props;
    // const { isDrawerOpen } = this.state;

    return (
      <div>
        <AppBar toggleDrawer={this.toggleDrawer} />
        <div className={classes.root}>{children}</div>
      </div>
    );
  }
}

export default withStyles(styles)(Layout);
