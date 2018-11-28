import React, { Component } from "react";
import PropTypes from "prop-types";
import TasksList from "../components/Tasks/List";
import { Theme, createStyles, withStyles } from "@material-ui/core/styles";
import Button from "@material-ui/core/Button";

const styles = ({ spacing }: Theme) =>
  createStyles({
    button: {
      margin: spacing.unit,
      float: "right"
    }
  });

interface TasksProps {
  classes: { [key: string]: any };
}

class Tasks extends Component<TasksProps, {}> {
  static propTypes = {
    classes: PropTypes.object.isRequired
  };
  render() {
    const { classes } = this.props;
    return (
      <div>
        <TasksList />
        <Button
          className={classes.button}
          onClick={() => console.log("add task")}>
          Add Task
        </Button>
      </div>
    );
  }
}

export default withStyles(styles)(Tasks);
