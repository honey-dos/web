import React, { Component } from "react";
import { Task, TaskModel } from "../lib/Task";
import { FetchContext } from "./FetchContext";

export interface TaskContextData {
  getTasks: () => Promise<Task[]>;
}

export const TaskContext = React.createContext<TaskContextData>({
  getTasks: async () => []
});

const { Provider, Consumer } = TaskContext;

export class TaskProvider extends Component {
  static contextType = FetchContext;

  async getTasks(): Promise<Task[]> {
    const { fetch } = this.context;
    const url = `api/todos/`;
    const tokenRequest = await fetch(url, {
      method: "GET",
      mode: "cors",
      cache: "no-cache"
    });
    const taskModels: TaskModel[] = await tokenRequest.json();
    const tasks = taskModels.map(i => new Task(i));
    return tasks;
  }

  render() {
    const { children } = this.props;

    return (
      <Provider
        value={{
          getTasks: () => this.getTasks()
        }}>
        {children}
      </Provider>
    );
  }
}

export const TaskConsumer = Consumer;
