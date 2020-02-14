import React from 'react';
import { Link } from 'react-router-dom';

interface TdProps {
    children: any;
    to: string;
}

const Td: React.FC<TdProps> = (props) => {
  // Conditionally wrapping content into a link
  const ContentTag = props.to ? Link : 'div';

  return (
    <td>
      <ContentTag to={props.to}>{props.children}</ContentTag>
    </td>
  );
}

export default Td;