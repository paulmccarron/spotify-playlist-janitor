import React, { ReactElement } from "react";
import { Tooltip as ReactTooltip } from "react-tooltip";

type TooltipProps = {
  content: string | ReactElement;
  children: ReactElement;
};

export const Tooltip = ({ content, children }: TooltipProps) => {
  const id = "tooltip-id-" + (Math.random() + 1).toString(36).substring(7);
  return (
    <>
      {React.cloneElement(children, { "data-tooltip-id": id })}
      <ReactTooltip id={id} style={{ backgroundColor: "black" }}>
        {content}
      </ReactTooltip>
    </>
  );
};
