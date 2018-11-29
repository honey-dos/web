import React, { Component } from "react";
import List from "@material-ui/core/List";
import TaskListItem from "./ListItem";
import TaskForm from "./Form";
import { Task, TaskFormModel } from "../../lib/Task";

interface TasksListProps {
  tasks: Task[];
  onTaskUpdate: (task: Task, taskFormModel: TaskFormModel) => void;
  onItemClick: (task: Task) => void;
}

interface TasksListState {
  editId: string;
}

const initialState: TasksListState = {
  editId: ""
};

class TasksList extends Component<TasksListProps, TasksListState> {
  constructor(props: TasksListProps) {
    super(props);
    this.state = initialState;
  }

  handleToggle = (task: Task) => {
    const { onItemClick } = this.props;
    onItemClick(task);
  };

  handleEdit(task: Task) {
    this.setState({ editId: task.id });
  }

  handleSave = (task: Task) => (taskFormModel: TaskFormModel) => {
    const { onTaskUpdate } = this.props;
    if (!taskFormModel) {
      return;
    }
    onTaskUpdate(task, taskFormModel);
    this.cancelEdit();
  };

  cancelEdit() {
    this.setState({ editId: "" });
  }

  render() {
    const { tasks } = this.props;
    const { editId } = this.state;
    return (
      <List>
        {tasks.map(task =>
          task.id === editId ? (
            <TaskForm
              key={task.id}
              task={task}
              onCancel={() => this.cancelEdit()}
              onSave={this.handleSave(task)}
            />
          ) : (
            <TaskListItem
              key={task.id}
              task={task}
              onClick={() => this.handleToggle(task)}
              onCheck={() => this.handleToggle(task)}
              onEdit={() => this.handleEdit(task)}
            />
          )
        )}
      </List>
    );
  }
}

export default TasksList;
