import React, { Component } from "react";
import List from "@material-ui/core/List";
import ListItem from "@material-ui/core/ListItem";
import ListItemText from "@material-ui/core/ListItemText";
import Checkbox from "@material-ui/core/Checkbox";
import ListItemSecondaryAction from "@material-ui/core/ListItemSecondaryAction";
import IconButton from "@material-ui/core/IconButton";
import EditIcon from "@material-ui/icons/Edit";

interface Task {
  id: string;
  name: string;
  checked: boolean;
  dueDate?: Date;
}

interface TasksListState {
  tasks: Task[];
}

const initialState: TasksListState = {
  tasks: [
    {
      id: "a",
      name: "Take out the garbage",
      checked: false,
      dueDate: new Date(2018, 10, 28, 8, 30)
    },
    {
      id: "b",
      name: "Pay the electric bill",
      checked: false,
      dueDate: new Date(2018, 11, 8, 18, 30)
    },
    {
      id: "c",
      name: "Paint the kitchen",
      checked: false
    },
    {
      id: "d",
      name: "Take wife out on date",
      checked: false
    }
  ]
};

class TasksList extends Component<{}, TasksListState> {
  constructor(props: {}) {
    super(props);
    this.state = initialState;
  }
  handleToggle = (id: string) => () => {
    const tasks = this.state.tasks;
    const task = tasks.find(tsk => tsk.id === id);
    if (!task) {
      return;
    }
    task.checked = !task.checked;
    this.setState({ tasks });
  };
  render() {
    const tasks = this.state.tasks;
    return (
      <List>
        {tasks.map(task => (
          <ListItem
            key={task.id}
            role={undefined}
            dense
            button
            onClick={this.handleToggle(task.id)}>
            <Checkbox checked={task.checked} tabIndex={-1} disableRipple />
            <ListItemText
              primary={task.name}
              secondary={task.dueDate ? task.dueDate.toLocaleString() : ""}
            />
            <ListItemSecondaryAction>
              <IconButton aria-label="Edit">
                <EditIcon />
              </IconButton>
            </ListItemSecondaryAction>
          </ListItem>
        ))}
      </List>
    );
  }
}

export default TasksList;
