package com.enjoy.mathero.ui.controller;


import com.enjoy.mathero.exceptions.InvalidRequestBodyException;
import com.enjoy.mathero.exceptions.UserServiceException;
import com.enjoy.mathero.service.CustomLobbyService;
import com.enjoy.mathero.service.UserService;
import com.enjoy.mathero.shared.MapUtils;
import com.enjoy.mathero.shared.dto.CustomLobbyDto;
import com.enjoy.mathero.shared.dto.QuestionDto;
import com.enjoy.mathero.shared.dto.UserDto;
import com.enjoy.mathero.ui.model.request.CustomLobbyRequestModel;
import com.enjoy.mathero.ui.model.request.QuestionRequestModel;
import com.enjoy.mathero.ui.model.response.CustomLobbyRest;
import com.enjoy.mathero.ui.model.response.ErrorMessages;
import com.enjoy.mathero.ui.model.response.QuestionRest;
import com.enjoy.mathero.ui.validator.CustomLobbyValidator;
import org.springframework.beans.BeanUtils;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.validation.BindingResult;
import org.springframework.web.bind.annotation.*;

import javax.validation.Valid;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

@RestController
@RequestMapping(path = "custom-lobbies")
public class CustomLobbyController {

    @Autowired
    CustomLobbyService customLobbyService;

    @Autowired
    UserService userService;

    @Autowired
    CustomLobbyValidator customLobbyValidator;

    @PostMapping
    public CustomLobbyRest createCustomLobby(@RequestBody CustomLobbyRequestModel customLobbyRequestModel, BindingResult result){
        customLobbyValidator.validate(customLobbyRequestModel, result);

        if(result.hasErrors())
            throw new InvalidRequestBodyException(result);

        CustomLobbyDto customLobbyDto = new CustomLobbyDto();

        UserDto authorDto = userService.getUserByUserId(customLobbyRequestModel.getAuthorId());
        customLobbyDto.setAuthorDetails(authorDto);

        List<QuestionRequestModel> questionRequestModels = customLobbyRequestModel.getQuestions();
        List<QuestionDto> questionDtos = new ArrayList<>();

        for(int i=0;i<questionRequestModels.size();i++){
            QuestionDto questionDto = new QuestionDto();
            BeanUtils.copyProperties(questionRequestModels.get(i), questionDto);
            questionDtos.add(questionDto);
        }

        customLobbyDto.setQuestions(questionDtos);
        CustomLobbyDto saved = customLobbyService.createCustomLobby(customLobbyDto);
        CustomLobbyRest returnValue = MapUtils.customLobbyDtoToCustomLobbyRest(saved);

        return returnValue;
    }

    @GetMapping(path = "/{lobbyId}")
    public CustomLobbyRest getCustomLobbyByLobbyId(@PathVariable String lobbyId){
        CustomLobbyDto customLobbyDto = customLobbyService.getCustomLobbyByLobbyId(lobbyId);

        CustomLobbyRest returnValue = MapUtils.customLobbyDtoToCustomLobbyRest(customLobbyDto);

        return returnValue;
    }

}
