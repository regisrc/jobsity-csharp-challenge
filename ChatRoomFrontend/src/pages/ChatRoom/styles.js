import styled from "styled-components";
import {ReactComponent as chevron} from "../../assets/chevron.svg";

export const Container = styled.div`
  display: flex;

  flex-direction: column;
  align-items: center;
  margin-top: 50px;

  width: 100vw;
  height: 100vh;
`;

export const ChatRoomCard = styled.div`
  display: flex;

  flex-direction: column;
  align-items: center;
  justify-content: center;

  flex-direction: row;

  width: 70%;
  height: 70px;

  margin-bottom: 15px;

  background-color: ${props => props.theme.colors.primaryBlue};

  border-radius: 15px;

  cursor: pointer;

  :hover {
    opacity: 90%;
  }

  @media (max-width: 1080px) {
    width: 90%;
  }
`;

export const Name = styled.h1`
  color: ${props => props.theme.colors.primaryWhite};
`;

export const Icon = styled(chevron)`
  path {
    fill: ${props => props.theme.colors.primaryWhite};
  }
`;

export const Feedback = styled.h1`
    color: ${props => props.theme.colors.primaryBlue};
`