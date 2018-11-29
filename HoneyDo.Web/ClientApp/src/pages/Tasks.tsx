import React, { Component } from "react";
import TasksList from "../components/Tasks/List";
import TaskForm from "../components/Tasks/Form";
import { Theme, createStyles, withStyles } from "@material-ui/core/styles";
import Button from "@material-ui/core/Button";
import { Task, TaskFormModel } from "../lib/Task";
import { TaskContext, TaskContextData } from "../contexts/TaskContext";

const styles = ({ spacing }: Theme) =>
  createStyles({
    button: {
      margin: spacing.unit,
      float: "right"
    },
    buttonContainer: {
      display: "flex",
      flexDirection: "row-reverse"
    }
  });

interface TasksProps {
  classes: { [key: string]: any };
}

interface TasksState {
  editTask?: Task;
  isFormOpen: boolean;
  tasks: Task[];
}

const initialState: TasksState = {
  isFormOpen: false,
  tasks: []
};

class Tasks extends Component<TasksProps, TasksState> {
  static contextType = TaskContext;

  constructor(props: TasksProps) {
    super(props);
    this.state = initialState;
  }

  async componentDidMount() {
    const { getTasks }: TaskContextData = this.context;
    const tasks = await getTasks();
    this.setState({ tasks });
  }

  handleSave = () => async (taskFormModel: TaskFormModel) => {
    const { createTask }: TaskContextData = this.context;
    const task = await createTask(taskFormModel);
    if (task) {
      const { tasks } = this.state;
      this.setState({ tasks: [...tasks, task] });
    }
    this.toggleForm();
  };

  toggleForm = () => {
    const isFormOpen = !this.state.isFormOpen;
    this.setState({ isFormOpen, editTask: undefined });
  };

  toggleCompleted = async (task: Task) => {
    const { completeTask }: TaskContextData = this.context;
    const updatedTask = await completeTask(task, !task.isCompleted());
    if (!updatedTask) {
      return;
    }
    this.updateExistingTask(updatedTask);
  };

  editTask = (task?: Task) => {
    this.setState({ editTask: task, isFormOpen: false });
  };

  handleTaskUpdate = async (task: Task, taskFormModel: TaskFormModel) => {
    const { updateTask }: TaskContextData = this.context;
    const updatedTask = await updateTask(task, taskFormModel);
    if (!updatedTask) {
      return;
    }
    this.updateExistingTask(updatedTask);
  };

  updateExistingTask = (task: Task) => {
    const { tasks } = this.state;
    const taskIndex = tasks.findIndex(tsk => tsk.id === task.id);
    const updateTasks: Task[] = [
      ...tasks.slice(0, taskIndex),
      task,
      ...tasks.slice(taskIndex + 1)
    ];
    this.setState({ tasks: updateTasks });
  };

  render() {
    const { classes } = this.props;
    const { isFormOpen, tasks, editTask } = this.state;
    return (
      <div>
        <TasksList
          editTask={editTask}
          tasks={tasks}
          onItemClick={task => this.toggleCompleted(task)}
          onItemEdit={task => this.editTask(task)}
          onCheck={task => this.toggleCompleted(task)}
          onCancelEdit={() => this.editTask()}
          onTaskUpdate={(task, taskFormModel) =>
            this.handleTaskUpdate(task, taskFormModel)
          }
        />
        {!isFormOpen ? (
          <div className={classes.buttonContainer}>
            <Button
              className={classes.button}
              onClick={() => this.toggleForm()}>
              Add Task
            </Button>
          </div>
        ) : (
          <TaskForm
            onCancel={() => this.toggleForm()}
            onSave={this.handleSave()}
          />
        )}
      </div>
    );
  }
}

export default withStyles(styles)(Tasks);
