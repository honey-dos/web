import React from 'react'
import PropTypes from 'prop-types'
import Button from '@material-ui/core/Button'

// We can inject some CSS into the DOM.
const styleDefault = {
  background: 'linear-gradient(45deg, #FE6B8B 30%, #FF8E53 90%)',
  borderRadius: 3,
  border: 0,
  color: 'white',
  height: 48,
  padding: '0 30px',
  boxShadow: '0 3px 5px 2px rgba(255, 105, 135, .3)'
}

const ColoredButton = (props: {
  style?: any
  children: any
  [key: string]: any
}) => {
  const { style, children, ...other } = props
  return (
    <Button style={{ ...styleDefault, ...style }} {...other}>
      {children}
    </Button>
  )
}

ColoredButton.propTypes = {
  children: PropTypes.node.isRequired,
  style: PropTypes.object
}

export default ColoredButton
