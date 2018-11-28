import React, { Component } from "react";
import List from "@material-ui/core/List";
import ListItem from "./ListItem";
import { Task } from "../../lib/Task";

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

  handleEdit = () => (task: Task) => {
    console.log(`editing task ${task.name}`);
  };

  render() {
    const tasks = this.state.tasks;
    return (
      <List>
        {tasks.map(task => (
          <ListItem
            key={task.id}
            task={task}
            onClick={this.handleToggle(task.id)}
            onCheck={this.handleToggle(task.id)}
            onEdit={this.handleEdit()}
          />
        ))}
      </List>
    );
  }
}

export default TasksList;
