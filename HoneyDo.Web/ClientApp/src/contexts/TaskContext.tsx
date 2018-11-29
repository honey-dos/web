import React, { Component } from "react";
import { Task, TaskModel, TaskFormModel } from "../lib/Task";
import { FetchContext, IFetch } from "./FetchContext";

export interface IGetTasks {
  (): Promise<Task[]>;
}

export interface ICreateTask {
  (taskFormModel: TaskFormModel): Promise<Task | null>;
}

export interface TaskContextData {
  getTasks: IGetTasks;
  createTask: ICreateTask;
}

export const TaskContext = React.createContext<TaskContextData>({
  getTasks: async () => [],
  createTask: async () => null
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

  render() {
    const { children } = this.props;
    fetch;

    return (
      <Provider
        value={{
          getTasks: () => this.getTasks(),
          createTask: taskFormModel => this.createTask(taskFormModel)
        }}>
        {children}
      </Provider>
    );
  }
}

export const TaskConsumer = Consumer;
