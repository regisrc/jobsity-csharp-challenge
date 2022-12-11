import styled from "styled-components";

export const Container = styled.div`
  display: flex;

  flex-direction: column;
  align-items: center;
  justify-content: center;

  width: 100vw;
  height: 100vh;
`;

export const LoginCard = styled.div`
  display: flex;

  flex-direction: column;
  align-items: center;
  justify-content: center;

  width: 40%;
  height: 500px;

  background-color: ${props => props.theme.colors.primaryBlue};

  border-radius: 15px;

  @media (max-width: 1080px) {
    width: 100%;
    border-radius: 0px;
  }
`;

export const Input = styled.input`
  font-size: 18px;
  padding: 10px;
  margin: 10px;

  width: 80%;

  color: ${props => props.theme.colors.primaryBlue};

  background: ${props => props.theme.colors.primaryWhite};
  border: none;
  border-radius: 3px;
  
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

export const TitleContainer = styled.div`
    width: 100%;

    display: flex;
    flex-direction: row;

    align-items: center;
    justify-content: center;
    margin-bottom: 10px;

    h1 {
        margin-right: 5px;
    }
`

export const Title = styled.h1`
  color: ${props => props.theme.colors.primaryWhite}; 
`;

export const TitleColored = styled.h1`
  color: ${props => props.theme.colors.primaryPurple}; 
  font-family: 'Guthen Bloots Personal Use';
`;

