import { Container, LoginCard, Input, Button, Title, TitleContainer, TitleColored } from "./styles";
import { useState } from "react";
import { useNavigate } from 'react-router-dom';
import axios from 'axios'

const Login = () => {
    const navigate = useNavigate();
    const [login, setLogin] = useState();
    const [password, setPassword] = useState();

    const Login = () => {
        const body = {
            login: login, 
            password: password
        }

        axios.post("https://localhost:7233/User/loginUser", body).then((response) => {
            if (response.data.success) {
                sessionStorage.setItem("token", response.data.token)
                sessionStorage.setItem("id", response.data.userId)
                alert("Logged")

                navigate('/home');
            }
          }).catch((response) =>{
                sessionStorage.removeItem("token")
                sessionStorage.removeItem("id")
                alert("Not Logged")
          });
    }

    return (
        <Container>
            <LoginCard>
                <TitleContainer>
                    <Title>Jobsity</Title>
                    <TitleColored>Challenge</TitleColored>
                </TitleContainer>
                <Input placeholder="Login" value={login} onChange={e => setLogin(e.target.value)}/>
                <Input type="password" placeholder="Password" value={password} onChange={e => setPassword(e.target.value)}/>
                <Button type="button" value="Login" onClick={Login}/>
            </LoginCard>
        </Container> 
    );
}

export default Login;