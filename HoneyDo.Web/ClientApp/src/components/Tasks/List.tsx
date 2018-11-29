import React, { Component } from "react";
import List from "@material-ui/core/List";
import TaskListItem from "./ListItem";
import TaskForm from "./Form";
import { Task, TaskFormModel } from "../../lib/Task";

interface TasksListProps {
  tasks: Task[];
  handleUpdate: (task: Task) => void;
  toggleCompleted: (task: Task) => void;
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
    const { toggleCompleted } = this.props;
    toggleCompleted(task);
  };

  handleEdit(task: Task) {
    this.setState({ editId: task.id });
  }

  handleSave = (task: Task) => (taskModel: TaskFormModel) => {
    const { handleUpdate } = this.props;
    if (!taskModel) {
      return;
    }
    handleUpdate({
      ...task,
      ...taskModel
    });
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
