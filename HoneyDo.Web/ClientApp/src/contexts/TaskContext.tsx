import React, { Component } from "react";
import { Task, TaskModel, TaskFormModel } from "../lib/Task";
import { FetchContext, IFetch } from "./FetchContext";

export interface IGetTasks {
  (): Promise<Task[]>;
}

export interface ICreateTask {
  (taskFormModel: TaskFormModel): Promise<Task | null>;
}

export interface ICompleteTask {
  (task: Task, isCompleted: boolean): Promise<Task | null>;
}

export interface TaskContextData {
  getTasks: IGetTasks;
  createTask: ICreateTask;
  completeTask: ICompleteTask;
}

export const TaskContext = React.createContext<TaskContextData>({
  getTasks: async () => [],
  createTask: async () => null,
  completeTask: async () => null
});

const { Provider, Consumer } = TaskContext;

export class TaskProvider extends Component {
  static contextType = FetchContext;

  getTasks: IGetTasks = async (): Promise<Task[]> => {
    const { fetch }: { fetch: IFetch } = this.context;
    const url = `api/todos/`;
    const tokenRequest = await fetch(url, {
      method: "GET",
      cache: "no-cache"
    });
    const taskModels: TaskModel[] = await tokenRequest.json();
    const tasks = taskModels.map(i => new Task(i));
    return tasks;
  };

  createTask: ICreateTask = async (
    taskFormModel: TaskFormModel
  ): Promise<Task> => {
    const { fetch }: { fetch: IFetch } = this.context;
    const url = `api/todos/`;
    const tokenRequest = await fetch(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json; charset=utf-8"
      },
      body: JSON.stringify(taskFormModel)
    });
    const taskModel: TaskModel = await tokenRequest.json();
    return new Task(taskModel);
  };

  completeTask: ICompleteTask = async (
    task: Task,
    isCompleted: boolean
  ): Promise<Task> => {
    const { fetch }: { fetch: IFetch } = this.context;
    const url = `api/todos/${task.id}/complete`;
    const method = isCompleted ? "PUT" : "DELETE";
    const tokenRequest = await fetch(url, {
      method
    });
    const taskModel: TaskModel = await tokenRequest.json();
    return new Task(taskModel);
  };

  render() {
    const { children } = this.props;
    fetch;

    return (
      <Provider
        value={{
          getTasks: () => this.getTasks(),
          createTask: taskFormModel => this.createTask(taskFormModel),
          completeTask: (task, isCompleted) =>
            this.completeTask(task, isCompleted)
        }}>
        {children}
      </Provider>
    );
  }
}

export const TaskConsumer = Consumer;
