import React, { Component, SyntheticEvent } from "react";
import Flatpickr from "react-flatpickr";
import "flatpickr/dist/themes/light.css";
import TextField from "@material-ui/core/TextField";
import InputAdornment from "@material-ui/core/InputAdornment";
import { IconButton } from "@material-ui/core";
import Backspace from "@material-ui/icons/Backspace";

interface Styles {
  [key: string]: React.CSSProperties;
}

const styles: Styles = {
  input: {
    minWidth: 260
  }
};

interface DatetimePickerProps {
  label?: string;
  date?: Date;
  onChange: (dates: Date[]) => void;
  className: string;
  fullWidth?: boolean;
  [key: string]: any;
}

export class DatetimePicker extends Component<DatetimePickerProps> {
  handleIconClick = (event: SyntheticEvent) => {
    const { onChange } = this.props;
    event.stopPropagation();
    onChange([]);
  };
  render() {
    const { label, date, onChange, className, fullWidth, ...rest } = this.props;
    return (
      <Flatpickr
        data-enable-time
        value={date}
        onChange={(date: Date[]) => onChange(date)}
        options={{ wrap: true }}
        {...rest}>
        <TextField
          data-input
          variant="outlined"
          label={label}
          value={date ? date.toLocaleString() : ""}
          margin="normal"
          type={"text"}
          className={className}
          fullWidth={fullWidth}
          InputProps={{
            style: styles.input,
            endAdornment: (
              <InputAdornment position="end">
                <IconButton data-clear>
                  <Backspace />
                </IconButton>
              </InputAdornment>
            )
          }}
        />
      </Flatpickr>
    );
  }
}
