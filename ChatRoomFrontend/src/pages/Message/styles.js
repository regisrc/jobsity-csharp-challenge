import styled from "styled-components";
import {ReactComponent as chevron} from "../../assets/chevron.svg";

export const Container = styled.div`
  display: flex;

  flex-direction: column;
  align-items: center;

  margin-top: 100px;

  width: 100vw;
  height: 100vh;
`;

export const ChatRoomCard = styled.div`
  display: flex;

  flex-direction: column;

  width: 70%;
  min-height: 50%;
  padding: 10px;

  margin-bottom: 15px;

  background-color: ${props => props.theme.colors.primaryBlue};

  @media (max-width: 1080px) {
    width: 100%;
  }
`;

export const UserName = styled.p`
  color: ${props => props.userColor};
  font-weight: normal;
  font-size: 20px;
  margin: 0px 5px;
`;

export const MessageWrited = styled.p`
  color: ${props => props.theme.colors.primaryWhite};
  font-weight: normal;
  font-size: 20px;
`;

export const MessageContainer = styled.div`
  display: flex;
  flex-direction: ${props => props.direction};

  width: 100%;
`

export const Input = styled.input`
  font-size: 18px;
  padding: 10px;
  margin: 10px;

  width: 70%;

  color: ${props => props.theme.colors.primaryBlue};

  background: ${props => props.theme.colors.primaryWhite};
  border: none;
  border-radius: 3px;

  @media (max-width: 1080px) {
    width: 100%;
  }
  
  ::placeholder {
    color: ${props => props.theme.colors.primaryBlue};
  }
`;

export const Button = styled.input`
  font-size: 18px;
  padding: 10px;
  margin: 10px;

  width: 30%;

  background: ${props => props.theme.colors.primaryYellow};
  border: none;
  border-radius: 3px;
  
  color: ${props => props.theme.colors.primaryBlack};
  
  :hover {
    opacity: 90%;
    cursor: pointer;
  }
`;