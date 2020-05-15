import React from "react";
import Dimmer from "semantic-ui-react/dist/commonjs/modules/Dimmer";
import { Loader } from "semantic-ui-react";

const LoadingComponent: React.FC<{ inverted?: boolean; content?: string }> = ({
  inverted = true,
  content,
}) => {
  return (
    <Dimmer active inverted={inverted}>
      <Loader content={content} />
    </Dimmer>
  );
};

export default LoadingComponent;
