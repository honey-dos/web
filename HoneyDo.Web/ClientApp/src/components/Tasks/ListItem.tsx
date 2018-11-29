import React from "react";
import { Task } from "../../lib/Task";
import ListItem from "@material-ui/core/ListItem";
import ListItemText from "@material-ui/core/ListItemText";
import Checkbox from "@material-ui/core/Checkbox";
import ListItemSecondaryAction from "@material-ui/core/ListItemSecondaryAction";
import IconButton from "@material-ui/core/IconButton";
import EditIcon from "@material-ui/icons/Edit";

interface ListItemProps {
  task: Task;
  onCheck: (task: Task) => void;
  onClick: (task: Task) => void;
  onEdit: (task: Task) => void;
}

const TaskListItem = ({ task, onCheck, onClick, onEdit }: ListItemProps) => (
  <ListItem role={undefined} dense button onClick={() => onClick(task)}>
    <Checkbox
      checked={task.isCompleted()}
      tabIndex={-1}
      onClick={event => {
        event.stopPropagation();
        onCheck(task);
      }}
      disableRipple
    />
    <ListItemText
      primary={task.name}
      secondary={task.dueDate ? task.dueDate.toLocaleString() : ""}
    />
    <ListItemSecondaryAction>
      <IconButton aria-label="Edit" onClick={() => onEdit(task)}>
        <EditIcon />
      </IconButton>
    </ListItemSecondaryAction>
  </ListItem>
);

export default TaskListItem;
