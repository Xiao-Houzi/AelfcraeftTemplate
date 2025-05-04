cmake_minimum_required(VERSION 3.15)

project(Survival)

# Set C++ standard
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# Update paths to the correct location of godot_cpp
set(GODOT_CPP_PATH "${CMAKE_SOURCE_DIR}/ThirdParty/godot_cpp")
set(GODOT_HEADERS_PATH "${GODOT_CPP_PATH}/include")

# Include directories
include_directories(
    ${GODOT_HEADERS_PATH}
    ${GODOT_HEADERS_PATH}/core
    ${GODOT_HEADERS_PATH}/gen
    ${GODOT_HEADERS_PATH}/classes
    ${GODOT_HEADERS_PATH}/variant
    ${GODOT_HEADERS_PATH}/gdextension
    ${GODOT_HEADERS_PATH}/templates
    ${GODOT_HEADERS_PATH}/node
    ${GODOT_HEADERS_PATH}/extension
    ${GODOT_HEADERS_PATH}/class_db
)

# Source files
file(GLOB SOURCE_FILES "src/*.cpp")

# Output library
add_library(RDCoreGameFunctionality SHARED ${SOURCE_FILES})

# Link Godot C++ bindings
target_link_libraries(RDCoreGameFunctionality PRIVATE ${GODOT_CPP_PATH}/bin/libgodot-cpp.${CMAKE_SHARED_LIBRARY_SUFFIX})

# Output paths
set_target_properties(RDCoreGameFunctionality PROPERTIES
    RUNTIME_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/bin"
    LIBRARY_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/bin"
    ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/bin"
)
